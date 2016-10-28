#!/bin/sh
mono ./src/Ps2PlantUmlRunner/bin/Release/Ps2PlantUmlRunner.exe $@ > output.plantuml
java -jar plantuml/plantuml.jar output.plantuml


