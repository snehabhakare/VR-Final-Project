# Features of the VR application

* Geo-spatial and socio-cultural information of the city
  ___Full Support__
  The source code of the prior work was used as a reference to procure the information of the city. All the gameobjects were made from scratch. All the models, materials, textures used are present [here](https://github.com/snehabhakare/VR-Final-Project/tree/master/Assets/Source/Models).
* Map Overlays (Point and click interactable icons)
  I have used VRTK scripts to make the gameobjects interactable and use them as icons for the map overlays. The path to these icons in the city scene is /Table1/Terrain/Icons and /Table2/Terrain/Icons. The logic to toggle the information is covered in this [script](https://github.com/snehabhakare/VR-Final-Project/blob/master/Assets/Source/Scripts/ToggleInfo.cs). 
  ___Full Support__
* Support for four configurations with varying depth of information presentation
  ___Full Support__
  This [script](https://github.com/snehabhakare/VR-Final-Project/blob/master/Assets/Source/Scripts/RunConfigurations.cs) handles the switch from one configuration to another.
* Analysis Tasks
  ___No Support__
  All the information of the city is nominal (no meaningful order exists). Also, all this information is fictional and was carefully created while avoiding any conflicting information to fulfill research purposes of the prior task. Hence, modifying the information to incorporate analysis tasks was beyond the scope of this project. Refer section 4.2.4 in the final project report for more details about analysis tasks.
  
Experiment Data: All the data collected during the experiments is present [here](https://github.com/snehabhakare/VR-Final-Project/tree/master/Assets/Source/Logs).
