#include "Services.hpp"
#include <functional>
#include <stdexcept>
#include <vector>
#include <algorithm>
#include <iostream>
#include <sstream>

double Services::TransactionService::getTax(Domain::OperationType operation, double transactionProfit) {
	auto allProfit = this->allProfit;

	auto taxCalculate = [operation, transactionProfit, allProfit]() -> double {
		switch (operation) {
		case Domain::OperationType::Buy:
			return 0.00;
		case Domain::OperationType::Sell:
			return allProfit < 0.00 ? 0.00 : transactionProfit * 0.2;
		default:
			throw std::invalid_argument("Invalid operation");
		}
		};

	return taxCalculate();
}

double Services::TransactionService::calculateBuyOperation(Domain::Transaction transaction) {
	this->allProfit = this->quantityAssets == 0 ? 0 : this->allProfit;
	this->weightedAverage = this->quantityAssets == 0 ? 0 : this->weightedAverage;
	this->weightedAverage = ((this->quantityAssets * this->weightedAverage) + (transaction.getQuantity() * transaction.getUnitCost())) / (this->quantityAssets + transaction.getQuantity());
	this->quantityAssets += transaction.getQuantity();

	return this->getTax(Domain::OperationType::Buy, 0);
}

double Services::TransactionService::calculateSellOperation(Domain::Transaction transaction) {
	this->quantityAssets -= transaction.getQuantity();

	auto sellLogic = [this](Domain::Transaction& transaction)-> double {
		auto operationCost = transaction.getQuantity() * transaction.getUnitCost();
		if (operationCost <= 20000) {
			if (transaction.getUnitCost() < this->weightedAverage)
				this->allProfit -= (transaction.getQuantity() * this->weightedAverage) - operationCost;

			return 0.0;
		}
		else {
			auto transactionProfit = operationCost - (transaction.getQuantity() * this->weightedAverage);
			auto oldAllProfit = this->allProfit;
			this->allProfit += transactionProfit;
			transactionProfit = oldAllProfit < 0 ? transactionProfit + oldAllProfit : transactionProfit;

			return transactionProfit > 0 ? this->getTax(Domain::OperationType::Sell, transactionProfit) : 0.00;
		}
		};
	return sellLogic(transaction);
}

std::vector<Domain::TaxResult> Services::TransactionService::calculateTaxes(std::vector<Domain::Transaction> transactions) {
	std::vector<Domain::TaxResult> taxResults;

	std::transform(transactions.begin(), transactions.end(), std::back_inserter(taxResults), [this](Domain::Transaction op) {
		auto taxResult = Domain::TaxResult();
		taxResult.setValue(std::round(op.getOperation() == Domain::OperationType::Buy ? this->calculateBuyOperation(op) : this->calculateSellOperation(op)));
		return taxResult;
		});

	return taxResults;
}

std::vector<std::string> Services::ApplicationCore::execute(std::vector<std::vector<Domain::Transaction>> transactions) {
	std::vector<std::string> results;
	results.reserve(transactions.size());

	for (const auto& transactionSet : transactions) {
		TransactionService transactionService;
		std::vector<Domain::TaxResult> taxes = transactionService.calculateTaxes(transactionSet);

		std::stringstream concatenatedString;
		concatenatedString << "[";

		for (auto result : taxes) {
			concatenatedString << result.asJson() << ",";
		}

		if (concatenatedString.tellp() > 1) {
			concatenatedString.seekp(-1, std::ios_base::end);
		}

		concatenatedString << "]";
		results.emplace_back(concatenatedString.str());
	}

	return results;
}