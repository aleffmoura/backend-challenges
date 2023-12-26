#include "Domain.hpp"
#include <sstream>

Domain::OperationType Domain::Transaction::getOperation() {
	return this->operation;
}
double Domain::Transaction::getUnitCost() {
	return this->unitCost;
}
long Domain::Transaction::getQuantity() {
	return this->quantity;
 }

void Domain::Transaction::setOperation(OperationType op) {
	this->operation = op;
}
void Domain::Transaction::setUnitCost(double unitCost) {
	this->unitCost = unitCost;
}
void Domain::Transaction::setQuantity(long quantity) {
	this->quantity = quantity;
}

double Domain::TaxResult::getValue() {
	return this->value;
}
void Domain::TaxResult::setValue(double value) {
	this->value = value;
}
std::string Domain::TaxResult::asJson() {
	std::ostringstream oss;
	oss << R"( { "tax": )" << this->getValue() << R"( })";
	return oss.str();;
}