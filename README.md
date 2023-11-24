# Mermaid Chart extension for IntelliJ IDEA

This extension is a Tool for visualizing and editing Mermaid diagrams in Visual Studio. The extension enables developers to view and edit diagrams stored in Mermaid Chart from Visual Studio. With the integration to the Mermaid Chart service, this extension allows users to attach diagrams to their code and to gain quick access to updating diagrams.

Simplify your development workflow with the Mermaid Chart extension.

## Features

In the explorer view under the MERMAIDCHART section you will find all the diagrams you have access to. When you click on a diagram, that diagram will be inserted into the code editor at the position of the cursor. To get the latest changes of diagrams from Mermaid Chart, click on the button named Refresh in the explorer view.

## Requirements

The Mermaid Chart extension for Visual Studio seamlessly integrates with the Mermaid Chart service, requiring an account to use. Choose between the free tier (limited to 5 diagrams) or the pro tier (unlimited diagrams). Collaborate by setting up teams and sharing diagrams within your development organisation. Simplify diagram management and enhance your workflow with Mermaid Chart for Visual Studio.

## Build Process

1. Import solution to Visual Studio
1. Select Release Configuration under Build - Configuration Manager...
1. Build Solution with Build - Build Solution (or hit Ctrl+Shift+B)
1. .vsix plugin file can be found in $SOLUTION_FOLDER/MermaidChart/bin/MermaidChart.vsix 

## Running Locally

1. Import solution to Visual Studio
1. Select Debug Configuration under Build - Configuration Manager...
1. Run the solution (> Current Instance on top bar or hit F5)