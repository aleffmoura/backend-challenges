#ifndef DOMAIN_HPP
#define DOMAIN_HPP

#include <iostream>

namespace Domain {

	enum class OperationType {
		Buy,
		Sell
	};

	class Transaction {
	private:
		OperationType operation;
		double unitCost = 0;
		long quantity = 0;
	public:
		OperationType getOperation();
		double getUnitCost();
		long getQuantity();

		void setOperation(OperationType);
		void setUnitCost(double);
		void setQuantity(long);
	};

	class TaxResult {
	private:
		double value = 0;
	public:
		double getValue();
		void setValue(double);
		std::string asJson();
	};
}

#endif 