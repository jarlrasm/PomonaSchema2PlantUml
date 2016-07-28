#!/bin/sh
mono ./PomonaSchema2PlantUml/bin/Debug/PomonaSchema2PlantUml.exe $@ > output.plantuml
java -jar plantuml/plantuml.jar output.plantuml


