# Unity Extension Easy Way Tools

![Header](/images/Long_Header.png)

**[Russian README](/README_ru.md)**

**Easy Way Tools** is Unity Extension for easy working with graphic assets. The following tools are now available:
* Batch Extract and Remap Materials from Selected 3D Models (FBX and OBJ)
* Batch Convert  DirectX Normal Maps to OpenGL Normal Maps directly in Unity
* Automatic Search and Assignment Textures in Material Slots (Based on Material Name and Suffixes of Texture Names)

***[Download Easy Way Tools](https://github.com/mrven/Unity-Easy-Way-Tools/raw/master/Releases/EasyWayTools_1_0.unitypackage)***
***[English Documentation (PDF)](https://github.com/mrven/Unity-Easy-Way-Tools/raw/master/Releases/Easy_Way_Tools_1_0_En.pdf)***
***[Russion Documentation (PDF)](https://github.com/mrven/Unity-Easy-Way-Tools/raw/master/Releases/Easy_Way_Tools_1_0_Ru.pdf)***

If you want to support me you can buy this addon on ***[Gumroad (Pay what you want)](https://gumroad.com/mrven)***


# INSTALLATION

For installation Easy Way Tools You need import EasyWayTools_xxx.unitypackage in your Unity Project (Assets -> Import Package -> Custom Package...).

# SETTINGS WINDOW AND TOOLS MENU

After Installation of Easy Way Tools will be added menu item Tools->Easy Way Tools -> Settings.

![Tool Settings](/images/01.png)

This menu item show “Easy Way Tools Settings” window. Settings for each instrument are described below.

Tools will be added to Assets -> Easy Way Tools menu (RMB in Project window).

![Tools Menu](/images/02.png)

# CONVERT NORMAL MAP TOOL

Convert Normal Map Tool is a tool for conversion of DirectX Normal Map to OpenGL Normal Map (or vice versa, if use this tool with OpenGL Normal Map). After using the tool will be created new texture with name SourceTextureName_OGL.png in same folder what is source image. All Texture Import settings (Resolution, Compression Method, etc.) will be copied from source image.

For using the tool select one or several Normal Map Texture(s) in Project window (if was selected non Normal Map textures and other types assets, they will be ignored), Click RMB and select Easy Way Tools -> Convert Normal Map.

Convert Normal Map Tool have no settings.

![Convert Normal before](/images/03.png)

![Convert Normal after](/images/04.png)

# EXTRACT AND REMAP MATERIALS TOOL

Extract and Remap Materials Tool for batch extraction and remapping materials from imported 3D models. Unity try find existing material in project and remap it on model, if material not found material will be extracted extract from model.

The tool has some settings in “Easy Way Tools Settings” window.

![Extract Marerials Settings](/images/05.png)

**1. Material Search** - where to search material for remap. Available 3 options:
* **Project Wide (default)** - search in all project.
* **Recursive Up** - search recursively up from model folder.
* **Local Folder** - search in model folder.

**2. Material Name** - material name for searching and remapping. Available 3 options:
* **From Model Material (default)** - material names from model’s material names. Preferred option.
* **Model Name And Model Material** - material name is Model_Name-Material_Name.
* **By Base Texture Name** - material names from main assigned texture (if texture was assigned in 3D software), otherwise used model’s material name.

**3. Move Extracted Materials to Folder** - move extracted materials to folder (folder path below (4)).  If disable this option materials will be extracted to folder Materials in same folder what is 3D model.

**4. Folder's path for extracted materials.** If folder not exist or folder location not in the project, materials will be extracted to folder Materials in same folder what is 3D model.

Example of using Extract and Remap Materials Tool:

We have 5 3D models with 4 assigned materials.

![Source Models](/images/06.png)

Select models, click RMB and select Easy Way Tools -> Extract and Remap Materials

![Extract Menu](/images/07.png)

After using Tool we have 4 materials in folder Assets/Materials and they assigned to models correctly.

![Extracted Naterials](/images/08.png)

![Remapped Naterials](/images/09.png)

If in the project exist materials which are match with model’s materials, they will be remapped. Another materials of models will be extracted in the project.

# TEXTURE ASSIGNMENT TOOL

Texture Assignment Tool is the tool for automatic assignment textures to slots of material (based on texture’s names). By default available profiles for Standard, Standard Specular, HDRP Lit и URP Lit shaders. Also You can add own profiles.

For example we have 5 materials with Standard shader.

![Objects](/images/10.png)

After using Texture Assignment Tool textures were assigned to materials automatically.

![Materials after using tool](/images/11.png)

***If You use tool with shaders with custom UI Editor (when editor set Keywords after assignment texture to slot) might you will have problem with update shader. For fixing this need deselect material and click on material icon. Automatic setting the Keywords implemented for main slots of Standard, Standard Specular, HDRP Lit и URP Lit shaders (Normal, Metallic/Smoothness, Specular, Mask and Emission).***

### Naming rules for correct working of Texture Assignment Tool

For correct working of Texture Assignment Tool must follow next rules for naming textures:
* Texture name have to start from (or contains) material name (depends from tool settings). For Example, for material Table will be found textures with names like Table_Normal, TableAO, etc. If tool setting “Texture Search Method” set to “Contains Material Name”, also will be found textures with names like New_Table_Albedo, NewTableAO, tableMesh_Table_Metallic. Searching is case sensitive!
* Texture name have to end with identifier of texture type. For Example, Table_MetallicSmoothness, tableMesh_Table_Albebo, TableNormal, etc. Searching is case sensitive!

### Tool settings and profiles for Texture Assignment Tool

Tool Settings available in Tools -> Easy Way Tools -> Settings.

![Assignment settings](/images/12.png)

**1. Texture Search Method** - Method for searching texture matching with material. Available 2 methods:
* **Starts With Material Name** - texture name **starts with** material name.
* **Contains Material Name** - texture name **contains** material name.

**2. Assignment Profiles** - selection of assignment profile for viewing and deleting. Also You can add new profile if select item Add New Profile… More info about creation own profiles in chapter “Creation new Texture Assignment Tool profile”.

**3. Shader Full Name** - full name of a shader which displaying in field “Shader” in a material. This name use for searching assignment profile matching selected material. Used FULL name of shader.

![Shader Name](/images/13.png)

**4. Material Slot/Texture Name** - matching between material slot and identifier of type in texture name. See below configuration of Texture Assignment Tool profile for Standard shader (available by default).

![Material Slots](/images/14.png)

**Material Slot** - slot’s identifier (property name) of material.

**Texture Name** - list of identifiers of texture types, which match with slot.

For Example, for slot _MainTex (Albedo) will be matched textures with names Chair_Albebo, Chair_AlbedoTransparency, ChairDiffuse, Chair_DiffuseTransparency, ChairBaseColor, Chair_Color, Chair_BaseColorMap, ChairColorMap, etc.

For slot _MetallicGlossMap (Metallic/Smoothness) will be matched texture with names Chair_Metallic, Chair_MetallicSmoothness, ChairMetallicMap, ChairMetallicSmooth, etc.

and also for other slots.

### Deletion of Texture Assignment Tool profile

For deletion of existing profile select profile in field Assignment Profiles (1) in window Easy Way Tools Settings and press button Delete This Profile (2).

![Delete Profile](/images/15.png)
