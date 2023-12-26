#include <iostream>
#include <nlohmann/json.hpp>
#include "Domain.hpp"
#include "Services.hpp"
#include "gtest/gtest.h"

using json = nlohmann::json;

std::vector<Domain::Transaction> deserialize_object(const std::string& linha) {
	try {
		auto transactionsJson = json::parse(linha);

		if (transactionsJson.is_array()) {
			std::vector<Domain::Transaction> transactions;
			transactions.reserve(transactionsJson.size());

			for (const auto& jsonObject : transactionsJson) {
				Domain::Transaction transaction;
				std::string operationStr = jsonObject["operation"].get<std::string>();
				transaction.setOperation(operationStr == "sell" ? Domain::OperationType::Sell : Domain::OperationType::Buy);
				transaction.setUnitCost(jsonObject["unit-cost"].get<double>());
				transaction.setQuantity(jsonObject["quantity"].get<long>());

				transactions.emplace_back(transaction);
			}

			return transactions;
		}

		return std::vector<Domain::Transaction>();
	}
	catch (const std::exception& e) {
		std::cerr << "Erro ao desserializar JSON: " << e.what() << std::endl;
		return std::vector<Domain::Transaction>();
	}
}

std::vector<std::vector<Domain::Transaction>> get_transactions() {
	std::string linha;
	std::getline(std::cin, linha);

	if (linha.empty() || linha.find_first_not_of(" \t\n\v\f\r") == std::string::npos) {
		return std::vector<std::vector<Domain::Transaction>>();
	}

	auto transactions = get_transactions();

	transactions.insert(transactions.begin(), deserialize_object(linha));

	return transactions;
}

int main(int argc, char** argv)
{
	Services::ApplicationCore app;

	std::vector<std::string> returned = app.execute(get_transactions());

	for (auto line : returned)
	{
		std::cout << line << std::endl;
	}

	::testing::InitGoogleTest(&argc, argv);
	return RUN_ALL_TESTS();
}