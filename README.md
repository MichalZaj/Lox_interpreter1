# Lox_Interpreter_Zajac
Implementation of the Lox programming language from the textbook: [*Crafting Interpreters* by Robert Nystrom](https://craftinginterpreters.com/) in C#.

# Chapter Progress
- The "Chapter_progress" folder contains the code for each chapter completed in the textbook. 
- The "Chapter_documentation" has text files for each chapter on progress made throughout the development. The text files include information on what was learned, what was implemented, and tests done. 
- For this assignment's preparation, the author(s) have utilized ChatGPT, a language model created by OpenAI. 
Within this assignment, the ChatGPT was used for purposes such as brainstorming, grammatical correction, writing paraphrasing, and learning.
# Install/Build
- Ensure that you have the .NET SDK installed on your machine. You can download it from the official [.NET website](https://dotnet.microsoft.com/en-us/download).
- Install Visual Studio on the [official Visual Studio website](https://visualstudio.microsoft.com/).
- In Visual Studio, click on the "Get Started" tab and go to "Open a project or solution." Find the "Lox_interpeter" folder that is in this repository.
- In the "Build" tab, click "clean solution" and then "build solution".
# Usage 
- Complete Install/Build.
- Run the interpreter by selecting the "Start" icon.
- After clicking start, a terminal window will showup where you can code in Lox!
# Testing
- Tests are in the "Lox_Interpreter/test" folder.
- To run the tests, go to the "Debug" tab in Visual Studio.
- In the "Debug" tab add the command line arguments "--test path/to/test/repo"
- Ex. of Command line arguments: "--test C:\Users\Micha\source\repos\Lox_Interpreter1\test"
- Click "Start" and the tests will run.
The following should be the end output:
```
Test 241/241 Passed
```
- The output of the tests on my machine is in the "TestOutput.txt" file : [Test Output](TestOutput.txt)
# Repository Layout
*   `Chapter_documentation/` – The .txt files explaining what was completed at each chapter.
*   `Chapter_progress/` - The code of the Interpreter developed chapter by chapter
*   `Lox_Interpreter/` - The Interpreter project folder with all necessary files.
*   `Lox_Interpreter/test/` - All test files.
*   `TestOutput.txt` – Output of the tests.
