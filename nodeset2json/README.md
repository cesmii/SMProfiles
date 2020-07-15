# Introduction 
This code generator tool converts a UA Nodeset to a proposed lightweight JSON structure for consideration.

# Pre-requisites
Currently this tool depends on Beeond's UMX Model eXcelerator Pro. Future versions may run independently.

# Getting Started
1.	Install UMX Pro from Beeond. A free trial license is available: https://beeond.net/umxpro/
2.  Clone this repo to a user directory of your choice then open and build the project in Visual Studio.
3.	Update the file generator.cfg in the UMX Pro install directory (usually C:\Program Files (x86)\Beeond\UMXPro\generator.cfg) to include this generator, by adding this line to the end of the file:

Nodeset to JSON,"C:\Users\your-clone-directory\bin\Debug\nodeset2json.bat" "$(NODESET_FILE_ALL_NODES)" "$(OUTPUT_DIR)output.json",true,true

4.	Run UMX Pro, add your namespace. Design a model, and save it.
5.	From the Project menu, choose Generate Code. Select the "Nodeset to JSON" generator.
6.  Select the output directory, and press the Generator button.
7.  Find the result as output.json in the selected directory.


