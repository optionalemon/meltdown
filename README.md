# Climate Change XR Education Escape Room: Meltdown

## Table of Contents

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
<img width="952" alt="Screenshot 2025-03-29 at 23 07 48" src="https://github.com/user-attachments/assets/ea4694cc-9414-4ca1-a9ab-089c38e0a7f0"/>

5) Then, under `Edit > Project Settings > Player`, ensure that `Active Input Handling` is set to  either `Both` or `Input System Package (New)`. Please restart your Unity if you are prompted to do so.
<img width="957" alt="Screenshot 2025-03-29 at 23 08 45" src="https://github.com/user-attachments/assets/74330ffb-556a-4745-a3be-4878f88ca381" />

6) Now, go to `File > Build Profiles`. Click on `Android` and click `Switch Platform`. If you are prompted to restart your Unity, please do so.
7) Ensure that there is the `Active` green box beside the `Android` option, as shown in the screenshot below.
<img width="730" alt="Screenshot 2025-03-29 at 23 16 05" src="https://github.com/user-attachments/assets/6c258ee6-138d-4c4d-b0b7-a31da73883ad" />

8) Plug in your VR device. Under `File > Build Profiles > Platform Settings`, find `Run Device` and set it to your Meta Quest.
9) Click `Build and Run` and enjoy the game!

## Assets Used

## Controls

## How to Play

## Team Members

## Feedback

Should there be any problems encountered with the experience, feel free to raise an issue in our repository! 

