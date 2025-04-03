
# Climate Change XR Education Escape Room: Meltdown

<p align="center">
  <img src="/ReadmeImages/meltdown.gif" width="70%" />
</p>


## Table of Contents
- [Climate Change XR Education Escape Room: Meltdown](#climate-change-xr-education-escape-room-meltdown)
  - [Table of Contents](#table-of-contents)
  - [About](#about)
  - [Setup](#setup)
  - [Assets Used](#assets-used)
  - [Key Features](#key-features)
    - [🥽 Tutorial Room](#-tutorial-room)
    - [🚀 Moving Around: Teleportation & Smooth Locomotion](#-moving-around-teleportation--smooth-locomotion)
    - [🤲 Grabbing Objects](#-grabbing-objects)
    - [🔍 Scanning Objects with an Educational Overlay](#-scanning-objects-with-an-educational-overlay)
    - [📜 Interacting with Posters](#-interacting-with-posters)
    - [🎶 Background Music & Audio](#-background-music--audio)
    - [🔊 Missed the Audio? No Problem!](#-missed-the-audio-no-problem)
  - [Team Members](#team-members)
  - [Feedback](#feedback)

## About

Meltdown is a VR escape room that teaches players about climate change in an interactive way :earth_asia:. Players go through a typical day, making choices that affect the environment ♻️. Pick the eco-friendly options to keep the world safe :white_check_mark: — but make bad choices, and you’ll see the consequences unfold! :warning: It’s a fun and eye-opening way to learn how our daily actions impact the planet :seedling:.

  

## Setup

Preparations:

- To play our game, ensure that the Unity version you are using is **6.40.1f**. There may be compatibility issues if run in other versions of Unity.

- Ensure that your Unity has the Android SDK downloaded.

- As for the hardware, this is developed and tested with **Meta Quest 2 & 3**, so you are recommended to use those for the hardware.

1) Clone our project with the following command: `git clone https://github.com/florentianayuwono/meltdown.git`

2) Open the project in Unity 6.40.1f.

3) Once the project has loaded, under `Project`, go to the `Assets folder > Scenes` and drag the `Tutorial Room` to the Hierarchy.

4) Go to `Edit > Project Settings > XR Plug-in Management > OpenXR`. Set `Render Mode` to `Multi-pass`.

<img  width="730"  alt="Screenshot 2025-03-29 at 23 07 48"  src="https://github.com/user-attachments/assets/ea4694cc-9414-4ca1-a9ab-089c38e0a7f0"/>

  

5) Then, under `Edit > Project Settings > Player`, ensure that `Active Input Handling` is set to either `Both` or `Input System Package (New)`. Please restart your Unity if you are prompted to do so.

<img  width="730"  alt="Screenshot 2025-03-29 at 23 08 45"  src="https://github.com/user-attachments/assets/74330ffb-556a-4745-a3be-4878f88ca381" />

  

6) Now, go to `File > Build Profiles`. Click on `Android` and click `Switch Platform`. If you are prompted to restart your Unity, please do so.

7) Ensure that there is the `Active` green box beside the `Android` option, as shown in the screenshot below.

<img  width="730"  alt="Screenshot 2025-03-29 at 23 16 05"  src="https://github.com/user-attachments/assets/6c258ee6-138d-4c4d-b0b7-a31da73883ad" />

  

8) Plug in your VR device. If you are using Windows, you can link your Meta Quest to the Meta Quest Link application with this [guide](https://www.meta.com/help/quest/509273027107091/).

10) If you are using Windows:

- Press the 'Play' button in Unity after linking up your Meta Quest with the Meta Quest Link app.

Otherwise:

1) Under `File > Build Profiles > Platform Settings`, find `Run Device` and set it to your Meta Quest.

2) Click `Build and Run` and enjoy the game!

  

## Assets Used

- [Scanner prefab](https://sketchfab.com/3d-models/sci-fi-scanner-00eee2c153504f67b5aa579c4b82dbd8)

- [VR Hands](https://drive.google.com/file/d/1B5qxgxok_B-kHy8oZSXuYUtrMea-BA1t/view)

- [Supermarket Room Template](https://www.cgtrader.com/free-3d-models/exterior/cityscape/convenience-store-f3032ffa-c5e7-4af4-af97-d9c912c7718a)

- [Skybox](https://assetstore.unity.com/packages/2d/textures-materials/sky/allsky-free-10-sky-skybox-set-146014)

- [Shopping cart](https://www.cgtrader.com/free-3d-models/food/miscellaneous/shopping-cart-d526e425-c861-44a9-866e-0d7ba41c68d7  )

- [Supermarket shelves](https://assetstore.unity.com/packages/3d/props/furniture/fresh-shelving-267101)

- [Eggs](https://www.cgtrader.com/free-3d-models/animals/other/3-brown-eggs)

- [UI Samples](https://assetstore.unity.com/packages/essentials/ui-samples-25468)

- [Disaster Room Lava Samples](https://assetstore.unity.com/packages/vfx/shaders/lava-flowing-shader-33635?srsltid=AfmBOoohwLEn4EW_2Hb1A2ZrAtUoCpASGz_WQ-wJXsNQufY_KLYCdpUz)

- Voiceovers: [hume.ai](https://www.hume.ai/) & [ElevenLabs](https://elevenlabs.io/)

  

## Key features

  

### 🥽 Tutorial Room

New to VR? 🚀 Don't fret! 😊 Our 🎮 Tutorial Room's got you covered—feel free to play around and get used to the controls! 🎯✨

<p align="center">
<img  src="/ReadmeImages/tutorialroom.gif" width="30%"/>
</p>
  

### 🚀 Moving Around: Teleportation &amp; Smooth Locomotion

Walk or teleport—move your way! 🏃‍♂️✨ Prefer smooth gliding? No worries! A vignette effect helps reduce motion sickness, so you stay comfy while exploring! 🌿😊
  
<p align="center">
<img  src="https://media.giphy.com/media/erTP0l9VUGZNHPVj16/giphy.gif" width="30%">
<img  src="https://media.giphy.com/media/hLej8lW0gHOYZEz44Q/giphy.gif" width="30%">
</p>

<p align="center">
<b>Left</b>: Teleportation | <b>Right</b>: Smooth Locomotion with Vignette
</p>

### 🤲 Grabbing Objects

Get hands-on! 👐 Pick up objects as you interact with the world around you!

  
<p align="center">
<img  src="/ReadmeImages/grab1.gif" width="30%"/>
</p>
  

### 🔍 Scanning Objects with an Educational Overlay

Scan &amp; learn! 📲✨ Point at food objects to reveal fun, bite-sized info in a sleek, eye-catching overlay!

  
<p align="center">
<img  src="https://media.giphy.com/media/1gr327VD6MYFvaBsoZ/giphy.gif" width="30%"/>
</p>
  
  

### 📜 Interacting with Posters

Posters aren’t just for looks! 🖼️ Get up close and interact with them for surprises, hints, and extra knowledge!

  
<p align="center">
<img  src="https://media.giphy.com/media/mUbMcMv1EwZXbrS8nr/giphy.gif" width="30%"/>
</p>
  

### 🎶 Background Music &amp; Audio

Immerse yourself! 🎧✨ A carefully crafted soundtrack and realistic audio effects bring the world to life—setting the perfect mood for your adventure! 🌱🔊

  

### 🔊 Missed the Audio? No Problem!

Never miss a thing! 💬 Subtitles ensure you catch every word, so you can focus on solving puzzles &amp; saving the planet! 🌍
  
<p align="center">
<img  src="/ReadmeImages/subtitles.gif" width="30%"/>
</p>
  

## Team Members

- Acacia Chong Xiao Xuan

- Florentiana Yuwono

- Melissa Anastasia Harijanto

- Xu Yi

  

## Feedback

  

Should there be any problems encountered with the experience, feel free to raise an issue in our repository!
