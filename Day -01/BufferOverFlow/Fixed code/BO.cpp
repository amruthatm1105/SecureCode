#include <iostream>
#include <string>

const std::string PASSWORD = "rictro";

int main()
{
    std::string input;

    std::cout << "Enter password: ";
    std::cin >> input;

    // Debug prints:
    std::cout << "Input: " << input << "\n";
    std::cout << "Password: " << PASSWORD << "\n";

    if (input == PASSWORD)
        std::cout << "Access granted\n";
    else
        std::cout << "Access denied\n";

    return 0;
}