#pragma once
#include <vector>
#include "Domain.hpp"

namespace Services{
	class TransactionService
	{
	private: 
		double weightedAverage = 0;
		double allProfit = 0;
		long quantityAssets = 0;

		double calculateBuyOperation(Domain::Transaction);
		double calculateSellOperation(Domain::Transaction);
		double getTax(Domain::OperationType, double);
	public:
		std::vector<Domain::TaxResult> calculateTaxes(std::vector<Domain::Transaction> transactions);
	};

	class ApplicationCore {
	public:
		std::vector<std::string> execute(std::vector<std::vector<Domain::Transaction>> transactions);
	};
}

