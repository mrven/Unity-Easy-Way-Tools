# Unity Extension Easy Way Tools

![Header](/images/Long_Header.png)

**[English README](/README.md)**

**Easy Way Tools** это расширение Unity, предназначенное для упрощения работы с графическими ассетами. На данный момент доступны следующие инструменты:
* Пакетный ремап и распаковка материалов из выделенных моделей (FBX или OBJ)
* Пакетное конвертирование Normal Map из формата DirectX в формат OpenGL внутри Unity
* Поиск и автоматизированное назначение текстур в слоты материалов (базируясь на имени материала и суффиксе в названии текстуры)

***[Скачать Easy Way Tools](https://github.com/mrven/Unity-Easy-Way-Tools/raw/master/Releases/EasyWayTools_1_0.unitypackage)***

***[Документация (английский) (PDF)](https://github.com/mrven/Unity-Easy-Way-Tools/raw/master/Releases/Easy_Way_Tools_1_0_En.pdf)***

***[Документация (русский) (PDF)](https://github.com/mrven/Unity-Easy-Way-Tools/raw/master/Releases/Easy_Way_Tools_1_0_Ru.pdf)***

Если вы хотите поддержать меня, то можете купить аддон на ***[Gumroad (Свободная цена)](https://gum.co/EasyWay)***

***[Смотреть обзор возможностей (Youtube Video)](https://www.youtube.com/watch?v=NrJfdVyL0eM)***


# УСТАНОВКА

Для установки Easy Way Tools достаточно импортировать EasyWayTools_xxx.unitypackage в свой Unity-проект (Assets -> Import Package -> Custom Package...).

# НАСТРОЙКИ И МЕНЮ ИНСТРУМЕНТОВ

После установки Easy Way Tools в верхнем меню Tools появится пункт Easy Way Tools -> Settings.

![Tool Settings](/images/01.png)

Данный пункт меню открывает окно с настройками расширения. Подробнее настройки для каждого инструмента описаны ниже.

Инструменты же добавляются в контекстное меню Assets -> Easy Way Tools (Правая кнопка мыши в окне Project).

![Tools Menu](/images/02.png)

# ИНСТРУМЕНТ CONVERT NORMAL MAP

Инструмент Convert Normal Map предназначен для конвертации текстур Normal Map формата DirectX в формат OpenGL (или наоборот, если применить инструмент к Normal Map формата OpenGL). После применения инструмента рядом с исходной текстурой будет создана новая сконвертированная текстура SourceTextureName_OGL.png. Все параметры импорта (разрешение, метод сжатия и т.д.) копируются с исходной текстуры на конвертированную.

Для применения инструмента выделите в окне Project одну или несколько карт нормалей (если будут выделены текстуры, у которых тип отличается от Normal и другие типы ассетов будут проигнорированы инструментом), нажмите правой кнопкой мыши и выберите Easy Way Tools -> Convert Normal Map.

Инструмент Convert Normal Map не имеет настроек.

![Convert Normal before](/images/03.png)

![Convert Normal after](/images/04.png)

# ИНСТРУМЕНТ EXTRACT AND REMAP MATERIALS

Инструмент Extract and Remap Materials предназначен для пакетной распаковки и ремапинга материалов на импортируемых моделях. При импорте Unity пытается сначала найти подходящий материал модели в проекте, если такого материала нет, то он распаковывается.

Инструмент имеет несколько настроек (для вызова настроек нажмите на пункт меню Tools -> Easy Way Tools -> Settings).

![Extract Marerials Settings](/images/05.png)

**1. Material Search** - область поиска материала для ремапинга. Доступно 3 варианта:
* **Project Wide (default)** - поиск по всему проекту.
* **Recursive Up** - поиск рекурсивно вверх (вверх по иерархии относительно локации модели).
* **Local Folder** - поиск в той же папке, где находится модель.

**2. Material Name** - имя материала для поиска и распаковки. Доступно 3 варианта:
* **From Model Material (default)** - имена материалов будут соответствовать именам материалов на модели. Предпочтительный вариант.
* **Model Name And Model Material** - имена будут иметь формат Имя_Модели-Имя_Материала.
* **By Base Texture Name** - имя материала будет соответствовать имени текстуры, которая была назначена на материал (если была), в противном случае используется имя материала.

**3. Move Extracted Materials to Folder** - перемещать распакованные файлы в папку, указанную в поле ниже (4).  Если же отключить данную опцию, материалы будут распаковываться в папку Materials, расположенную рядом с моделью.

**4. Путь к папке, куда будут распаковываться материалы.** Если указанной папки не существует или она находится за пределами проекта, материалы будут распаковываться в папку Materials, расположенную рядом с моделью.

Рассмотрим пример использования инструмента:

У нас есть 5 моделей с 4 материала, назначенными на эти модели.

![Source Models](/images/06.png)

Импортируем модели в Unity, настраиваем параметры Extract Materials From Models

![Extract Settings](/images/24.png)

Выделяем модели, нажимаем правой кнопкой и запускаем инструмент  Extract and Remap Materials

![Extract Menu](/images/07.png)

В результате мы получаем 4 материала в папке Assets/Materials, которые правильно назначены на наши объекты.

![Extracted Naterials](/images/08.png)

![Remapped Naterials](/images/09.png)

Если бы в проекте был один/несколько материал/ов, которые подходили бы для ремапинга (в соответствии с настройками инструмента), то использовались бы уже существующие материалы, а те, которые не были найдены в проекте - распаковались.

# ИНСТРУМЕНТ TEXTURE ASSIGNMENT TOOL

Инструмент Texture Assignment Tool позволяет автоматически найти текстуры для выделенных материалов и назначить их в нужные слоты. По умолчанию есть профили для шейдеров Standard, Standard Specular, HDRP Lit и URP Lit. Также можно добавлять собственные профили для любых шейдеров.

В примере ниже у объектов 5 материалов с шейдером Standard.

![Objects](/images/10.png)

Запустив Texture Assignment Tool мы получаем материалы с назначенными текстурами.

![Materials after using tool](/images/11.png)

***При работе с шейдерами с кастомным UI Editor (а точнее с назначением Keywords) могут быть проблемы с обновлением отображения шейдера. Чтобы это пофиксить, необходимо снять выделение с материала и снова кликнуть по нему. Автоматическое назначение необходимых Keywords прописано для основных карт шейдеров Standard, Standard Specular, HDRP Lit и URP Lit (Normal, Metallic/Smoothness, Specular, Mask and Emission).***

### Правила нейминга для корректной работы Texture Assignment Tool

Для корректной работы Texture Assignment Tool необходимо соблюдать некоторые правила именования текстур:
* Имя текстуры должно начинаться или содержать полное имя материала (в зависимости от настроек инструмента). Например, для материала Table попадают под поиск такие текстуры как Table_Normal, TableAO и т.д. Если в настройках установлен метод поиска “содержит имя материала”, то также в поиск включатся New_Table_Albedo, NewTableAO, tableMesh_Table_Metallic. Также надо учитывать, что поиск чувствителен к регистру!
* Имя текстуры должно заканчиваться идентификатором типа текстуры. Например, Table_MetallicSmoothness, tableMesh_Table_Albebo, TableNormal. Поиск чувствителен к регистру!

### Настройки инструмента и устройство профилей Texture Assignment Tool

Настройки инструмента расположены в пункте меню Tools -> Easy Way Tools -> Settings.

![Assignment settings](/images/12.png)

**1. Texture Search Method** - Texture Search Method - метод поиска текстур, соответствующих материалу. Доступно 2 метода:
* **Starts With Material Name** - имена текстур **начинаются** с имени материала.
* **Contains Material Name** - имена текстур **содержат** имя материала.

**2. Assignment Profiles** - выбор профиля назначения текстур для просмотра или удаления. Также можно добавить новый профиль, выбрав пункт Add New Profile… Подробнее добавление профилей описано в главе “Добавление нового профиля Texture Assignment Tool”.

**3. Shader Full Name** - полное имя шейдера, которое отображается в поле Shader в материале. Именно по этому полю происходит поиск профиля, соответствующего материалу. Используется ПОЛНЫЙ путь шейдера.

![Shader Name](/images/13.png)

**4. Material Slot/Texture Name** - соответствие слота материала с подходящих идентификаторов текстур. Рассмотрим подробнее устройство профилей Texture Assignment Tool на примере профиля Standard, доступного по умолчанию.

![Material Slots](/images/14.png)

**Material Slot** - идентификатор слота материала.

**Texture Name** - список идентификаторов типа текстуры, которые подходят для этого слота.

Например, для материала Chair в слот _MainTex (Albedo) подойдут текстуры с именами Chair_Albebo, Chair_AlbedoTransparency, ChairDiffuse, Chair_DiffuseTransparency, ChairBaseColor, Chair_Color, Chair_BaseColorMap, ChairColorMap и т.п.

В слот _MetallicGlossMap (Metallic/Smoothness) подойдут текстуры с именами Chair_Metallic, Chair_MetallicSmoothness, ChairMetallicMap, ChairMetallicSmooth и т.п.

и т.д. для остальных слотов.

### Удаление профилей Texture Assignment Tool

Для удаления существующего профиля в окне Easy Way Tools Settings выберите профиль в поле Assignment Profiles (1) и нажмите кнопку Delete This Profile (2).

![Delete Profile](/images/15.png)

### Добавление нового профиля Texture Assignment Tool

Для добавления нового профиля выберите в поле Assignment Profiles пункт Add New Profile… (1) 

![Create Profile](/images/16.png)

Для примера попробуем создать профиль для шейдера Bumped Diffuse. Для того, чтобы получить всю необходимую для профиля информацию выбираем материал с шейдером Legacy Shaders/Bumped Diffuse и в инспекторе нажимаем на шестерёнку и выбираем Select Shader. После этого в инспекторе откроется интересующий нас шейдер.

![Select Shader](/images/17.png)

В Profile Name задаём (2) произвольное имя профиля, а в Shader Full Name (3) вписываем полное название шейдера.

![Shader Full Name](/images/18.png)

Затем добавляем новый слот (4) нажатием кнопки Add New Material Slot (5). С помощью Delete Last Material Slot (6) можно удалить лишние добавленные слоты. Clear All Material Slots (7) удаляет ВСЕ добавленные слоты.

![Add Slot](/images/19.png)

Вписываем в левое поле идентификатор слота из инспектора, в правом поле перечисляем **ЧЕРЕЗ ЗАПЯТУЮ** подходящие идентификаторы текстур.

![Enter data to Slot 1](/images/20.png)

Повторяем тоже самое для другого слота. Добавляем и заполняем.

![Enter data to Slot 2](/images/21.png)

После того, как будет внесена информация для слотов (можно вписывать в профиль не все слоты, а только интересующие нас), нажимаем кнопку Save New Profile (8). Профиль будет сохранён и автоматически выбран для просмотра в окне Easy Way Tools Settings.

![View New Profile](/images/22.png)

# ВЕРСИЯ EASY WAY TOOLS

Версию установленного расширения можно посмотреть в нижней части окна Easy Way Tools Settings.

![Version](/images/23.png)

# ИСТОРИЯ ИЗМЕНЕНИЙ

Обновления отсутствуют.