#!/bin/sh
mono ./PomonaSchema2PlantUml/bin/Release/PomonaSchema2PlantUml.exe $@ > output.plantuml
java -jar plantuml/plantuml.jar output.plantuml


