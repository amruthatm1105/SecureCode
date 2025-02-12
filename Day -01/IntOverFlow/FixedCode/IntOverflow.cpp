#include <iostream>
#include <climits> 


bool safe_add(int a, int b, int& result) {
    if (a > 0 && b > 0) {
       
        if (a > INT_MAX - b) {
            return false; 
        }
    }
    else if (a < 0 && b < 0) {
       
        if (a < INT_MIN - b) {
            return false;
        }
    }
    result = a + b;
    return true; s
}

int main() {
    int x, y;
    std::cout << "Enter the first integer (x): ";
    std::cin >> x;
    std::cout << "Enter the second integer (y): ";
    std::cin >> y;

    int result;
    if (safe_add(x, y, result)) {
        std::cout << "Result: " << result << std::endl;
    }
    else {
        std::cout << "Error: Integer overflow occurred!" << std::endl;
    }

    return 0;
}