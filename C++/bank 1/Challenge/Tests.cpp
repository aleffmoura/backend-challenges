#define _SILENCE_TR1_NAMESPACE_DEPRECATION_WARNING
#include "gtest/gtest.h"
#include "Services.hpp"
#include "Domain.hpp"

TEST(TransactionTaxDomainServiceTests, CaseOne_ShouldBeOk) {
	Services::TransactionService service;

	double costBuy = 10.00;
	double costSell = 10.00;
	int quantityBuy = 100;
	int quantitySell = 50;

	auto createTransaction = [](Domain::OperationType type, long quantity, double cost) -> Domain::Transaction {
		Domain::Transaction transactionBull;
		transactionBull.setOperation(type);
		transactionBull.setQuantity(quantity);
		transactionBull.setUnitCost(cost);

		return transactionBull;
		};


	std::vector<Domain::Transaction> transactions;
	transactions.reserve(3);
	transactions.emplace_back(createTransaction(Domain::OperationType::Buy, quantityBuy, costBuy));
	transactions.emplace_back(createTransaction(Domain::OperationType::Sell, quantitySell, costSell));
	transactions.emplace_back(createTransaction(Domain::OperationType::Sell, quantitySell, costSell));

	// Action
	auto outputs = service.calculateTaxes(transactions);

	// Verifies
	ASSERT_FALSE(outputs.empty());
	ASSERT_EQ(outputs.size(), transactions.size());
	ASSERT_DOUBLE_EQ(outputs[0].getValue(), 0);
	ASSERT_DOUBLE_EQ(outputs[1].getValue(), 0);
	ASSERT_DOUBLE_EQ(outputs[2].getValue(), 0);
}

TEST(TransactionTaxDomainServiceTests, CaseTwo_ShouldBeOk) {
	Services::TransactionService service;

	double costBuy = 10;
	int quantityBuy = 10000;
	int quantitySell = 5000;

	auto createTransaction = [](Domain::OperationType type, long quantity, double cost) -> Domain::Transaction {
		Domain::Transaction transactionBull;
		transactionBull.setOperation(type);
		transactionBull.setQuantity(quantity);
		transactionBull.setUnitCost(cost);

		return transactionBull;
		};


	std::vector<Domain::Transaction> transactions;
	transactions.reserve(3);
	transactions.emplace_back(createTransaction(Domain::OperationType::Buy, quantityBuy, costBuy));
	transactions.emplace_back(createTransaction(Domain::OperationType::Sell, quantitySell, 20.00));
	transactions.emplace_back(createTransaction(Domain::OperationType::Sell, quantitySell, 5.00));

	// Action
	auto outputs = service.calculateTaxes(transactions);

	// Verifies
	ASSERT_FALSE(outputs.empty());
	ASSERT_EQ(outputs.size(), transactions.size());
	ASSERT_DOUBLE_EQ(outputs[0].getValue(), 0);
	ASSERT_DOUBLE_EQ(outputs[1].getValue(), 10000);
	ASSERT_DOUBLE_EQ(outputs[2].getValue(), 0);
}

TEST(TransactionTaxDomainServiceTests, CaseOneAndTwo_ShouldBeOk) {
	Services::TransactionService service;

	auto createTransaction = [](Domain::OperationType type, long quantity, double cost) -> Domain::Transaction {
		Domain::Transaction transactionBull;
		transactionBull.setOperation(type);
		transactionBull.setQuantity(quantity);
		transactionBull.setUnitCost(cost);

		return transactionBull;
		};


	std::vector<Domain::Transaction> transactions1;
	transactions1.reserve(3);
	transactions1.emplace_back(createTransaction(Domain::OperationType::Buy, 100, 10.00));
	transactions1.emplace_back(createTransaction(Domain::OperationType::Sell, 50, 15.00));
	transactions1.emplace_back(createTransaction(Domain::OperationType::Sell, 50, 15.00));

	std::vector<Domain::Transaction> transactions2;
	transactions2.reserve(3);
	transactions2.emplace_back(createTransaction(Domain::OperationType::Buy, 10000, 10.00));
	transactions2.emplace_back(createTransaction(Domain::OperationType::Sell, 5000, 20.00));
	transactions2.emplace_back(createTransaction(Domain::OperationType::Sell, 5000, 5.00));

	// Action
	auto outputs1 = service.calculateTaxes(transactions1);
	auto outputs2 = service.calculateTaxes(transactions2);

	// Verifies
	ASSERT_FALSE(outputs1.empty());
	ASSERT_EQ(outputs1.size(), transactions1.size());
	ASSERT_DOUBLE_EQ(outputs1[0].getValue(), 0);
	ASSERT_DOUBLE_EQ(outputs1[1].getValue(), 0);
	ASSERT_DOUBLE_EQ(outputs1[2].getValue(), 0);

	ASSERT_FALSE(outputs2.empty());
	ASSERT_EQ(outputs2.size(), transactions2.size());
	ASSERT_DOUBLE_EQ(outputs2[0].getValue(), 0);
	ASSERT_DOUBLE_EQ(outputs2[1].getValue(), 10000);
	ASSERT_DOUBLE_EQ(outputs2[2].getValue(), 0);
}

TEST(TransactionTaxDomainServiceTests, CaseThree_ShouldBeOk) {
	Services::TransactionService service;

	auto createTransaction = [](Domain::OperationType type, long quantity, double cost) -> Domain::Transaction {
		Domain::Transaction transactionBull;
		transactionBull.setOperation(type);
		transactionBull.setQuantity(quantity);
		transactionBull.setUnitCost(cost);

		return transactionBull;
		};


	std::vector<Domain::Transaction> transactions;
	transactions.reserve(3);
	transactions.emplace_back(createTransaction(Domain::OperationType::Buy, 10000, 10.00));
	transactions.emplace_back(createTransaction(Domain::OperationType::Sell, 5000, 5.00));
	transactions.emplace_back(createTransaction(Domain::OperationType::Sell, 3000, 20.00));

	// Action
	auto outputs = service.calculateTaxes(transactions);

	// Verifies
	ASSERT_FALSE(outputs.empty());
	ASSERT_EQ(outputs.size(), transactions.size());
	ASSERT_DOUBLE_EQ(outputs[0].getValue(), 0);
	ASSERT_DOUBLE_EQ(outputs[1].getValue(), 0);
	ASSERT_DOUBLE_EQ(outputs[2].getValue(), 1000);
}

TEST(TransactionTaxDomainServiceTests, CaseFour_ShouldBeOk) {
	Services::TransactionService service;

	auto createTransaction = [](Domain::OperationType type, long quantity, double cost) -> Domain::Transaction {
		Domain::Transaction transactionBull;
		transactionBull.setOperation(type);
		transactionBull.setQuantity(quantity);
		transactionBull.setUnitCost(cost);

		return transactionBull;
		};


	std::vector<Domain::Transaction> transactions;
	transactions.reserve(3);
	transactions.emplace_back(createTransaction(Domain::OperationType::Buy, 10000, 10.00));
	transactions.emplace_back(createTransaction(Domain::OperationType::Buy, 5000, 25.00));
	transactions.emplace_back(createTransaction(Domain::OperationType::Sell, 10000, 15.00));

	// Action
	auto outputs = service.calculateTaxes(transactions);

	// Verifies
	ASSERT_FALSE(outputs.empty());
	ASSERT_EQ(outputs.size(), transactions.size());
	ASSERT_DOUBLE_EQ(outputs[0].getValue(), 0);
	ASSERT_DOUBLE_EQ(outputs[1].getValue(), 0);
	ASSERT_DOUBLE_EQ(outputs[2].getValue(), 0);
}


TEST(TransactionTaxDomainServiceTests, CaseFive_ShouldBeOk) {
	Services::TransactionService service;

	auto createTransaction = [](Domain::OperationType type, long quantity, double cost) -> Domain::Transaction {
		Domain::Transaction transactionBull;
		transactionBull.setOperation(type);
		transactionBull.setQuantity(quantity);
		transactionBull.setUnitCost(cost);

		return transactionBull;
		};


	std::vector<Domain::Transaction> transactions;
	transactions.reserve(3);
	transactions.emplace_back(createTransaction(Domain::OperationType::Buy, 10000, 10.00));
	transactions.emplace_back(createTransaction(Domain::OperationType::Buy, 5000, 25.00));
	transactions.emplace_back(createTransaction(Domain::OperationType::Sell, 10000, 15.00));
	transactions.emplace_back(createTransaction(Domain::OperationType::Sell, 5000, 25.00));

	// Action
	auto outputs = service.calculateTaxes(transactions);

	// Verifies
	ASSERT_FALSE(outputs.empty());
	ASSERT_EQ(outputs.size(), transactions.size());
	ASSERT_DOUBLE_EQ(outputs[0].getValue(), 0);
	ASSERT_DOUBLE_EQ(outputs[1].getValue(), 0);
	ASSERT_DOUBLE_EQ(outputs[2].getValue(), 0);
	ASSERT_DOUBLE_EQ(outputs[3].getValue(), 10000);
}

TEST(TransactionTaxDomainServiceTests, CaseSix_ShouldBeOk) {
	Services::TransactionService service;

	auto createTransaction = [](Domain::OperationType type, long quantity, double cost) -> Domain::Transaction {
		Domain::Transaction transactionBull;
		transactionBull.setOperation(type);
		transactionBull.setQuantity(quantity);
		transactionBull.setUnitCost(cost);

		return transactionBull;
		};


	std::vector<Domain::Transaction> transactions;
	transactions.reserve(3);
	transactions.emplace_back(createTransaction(Domain::OperationType::Buy, 10000, 10.00));
	transactions.emplace_back(createTransaction(Domain::OperationType::Sell, 5000, 2.00));
	transactions.emplace_back(createTransaction(Domain::OperationType::Sell, 2000, 20.00));
	transactions.emplace_back(createTransaction(Domain::OperationType::Sell, 2000, 20.00));
	transactions.emplace_back(createTransaction(Domain::OperationType::Sell, 1000, 25.00));

	// Action
	auto outputs = service.calculateTaxes(transactions);

	// Verifies
	ASSERT_FALSE(outputs.empty());
	ASSERT_EQ(outputs.size(), transactions.size());
	ASSERT_DOUBLE_EQ(outputs[0].getValue(), 0);
	ASSERT_DOUBLE_EQ(outputs[1].getValue(), 0);
	ASSERT_DOUBLE_EQ(outputs[2].getValue(), 0);
	ASSERT_DOUBLE_EQ(outputs[3].getValue(), 0);
	ASSERT_DOUBLE_EQ(outputs[4].getValue(), 3000);
}

TEST(TransactionTaxDomainServiceTests, CaseSeven_ShouldBeOk) {
	Services::TransactionService service;

	auto createTransaction = [](Domain::OperationType type, long quantity, double cost) -> Domain::Transaction {
		Domain::Transaction transactionBull;
		transactionBull.setOperation(type);
		transactionBull.setQuantity(quantity);
		transactionBull.setUnitCost(cost);

		return transactionBull;
		};


	std::vector<Domain::Transaction> transactions;
	transactions.reserve(3);
	transactions.emplace_back(createTransaction(Domain::OperationType::Buy, 10000, 10.00));
	transactions.emplace_back(createTransaction(Domain::OperationType::Sell, 5000, 2.00));
	transactions.emplace_back(createTransaction(Domain::OperationType::Sell, 2000, 20.00));
	transactions.emplace_back(createTransaction(Domain::OperationType::Sell, 2000, 20.00));
	transactions.emplace_back(createTransaction(Domain::OperationType::Sell, 1000, 25.00));
	transactions.emplace_back(createTransaction(Domain::OperationType::Buy, 10000, 20.00));
	transactions.emplace_back(createTransaction(Domain::OperationType::Sell, 5000, 15.00));
	transactions.emplace_back(createTransaction(Domain::OperationType::Sell, 4350, 30.00));
	transactions.emplace_back(createTransaction(Domain::OperationType::Sell, 650, 30.00));

	// Action
	auto outputs = service.calculateTaxes(transactions);

	// Verifies
	ASSERT_FALSE(outputs.empty());
	ASSERT_EQ(outputs.size(), transactions.size());
	ASSERT_DOUBLE_EQ(outputs[0].getValue(), 0);
	ASSERT_DOUBLE_EQ(outputs[1].getValue(), 0);
	ASSERT_DOUBLE_EQ(outputs[2].getValue(), 0);
	ASSERT_DOUBLE_EQ(outputs[3].getValue(), 0);
	ASSERT_DOUBLE_EQ(outputs[4].getValue(), 3000);
	ASSERT_DOUBLE_EQ(outputs[5].getValue(), 0);
	ASSERT_DOUBLE_EQ(outputs[6].getValue(), 0);
	ASSERT_DOUBLE_EQ(outputs[7].getValue(), 3700);
	ASSERT_DOUBLE_EQ(outputs[8].getValue(), 0);
}

TEST(TransactionTaxDomainServiceTests, CaseEight_ShouldBeOk) {
	Services::TransactionService service;

	auto createTransaction = [](Domain::OperationType type, long quantity, double cost) -> Domain::Transaction {
		Domain::Transaction transactionBull;
		transactionBull.setOperation(type);
		transactionBull.setQuantity(quantity);
		transactionBull.setUnitCost(cost);

		return transactionBull;
		};


	std::vector<Domain::Transaction> transactions;
	transactions.reserve(3);
	transactions.emplace_back(createTransaction(Domain::OperationType::Buy, 10000, 10.00));
	transactions.emplace_back(createTransaction(Domain::OperationType::Sell, 10000, 50.00));
	transactions.emplace_back(createTransaction(Domain::OperationType::Buy, 10000, 20.00));
	transactions.emplace_back(createTransaction(Domain::OperationType::Sell, 10000, 50.00));

	// Action
	auto outputs = service.calculateTaxes(transactions);

	// Verifies
	ASSERT_FALSE(outputs.empty());
	ASSERT_EQ(outputs.size(), transactions.size());
	ASSERT_DOUBLE_EQ(outputs[0].getValue(), 0);
	ASSERT_DOUBLE_EQ(outputs[1].getValue(), 80000);
	ASSERT_DOUBLE_EQ(outputs[2].getValue(), 0);
	ASSERT_DOUBLE_EQ(outputs[3].getValue(), 60000);
}