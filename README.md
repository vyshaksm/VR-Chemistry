# VR Molecular Chemistry Lab

## Overview
A Unity XR project where players can grab atoms in VR, combine them into valid molecules, and view discovered molecules through world-space UI panels.

## Features
- XR grab interaction for atom and molecule objects
- Trigger-based atom-to-atom, atom-to-molecule, and molecule-to-molecule combination flow
- Molecule visualization with formula and bond details
- Molecule discovery library UI
- Recipe panel toggle mapped to the left controller secondary button
- Reset system for clearing active atoms and molecules
- Audio feedback on successful molecule discovery

## Tech Stack
- Unity 6
- XR Interaction Toolkit
- OpenXR
- Meta OpenXR
- Universal Render Pipeline (URP)
- TextMeshPro
- DOTween

## Unity Version
- `6000.3.10f1`

## Project Structure
- `Assets/_Scripts` - gameplay, XR interaction, molecule logic, and UI scripts
- `Assets/_Prefabs` - atom, molecule, and UI prefabs
- `Assets/_Scenes` - main scene setup
- `Assets/_Scriptable Objects` - atom and molecule data assets
- `ProjectSettings` - Unity project configuration

## Setup
1. Open the project in Unity `6000.3.10f1`.
2. Open `Assets/_Scenes/SampleScene.unity`.
3. Let Unity import packages and compile scripts.
4. Confirm XR packages are installed through the Package Manager.
5. Press Play in the editor or build for Android/Meta Quest.

## Controls
- Grab atoms using XR controllers.
- Bring atoms or molecules close together to trigger combination checks.
- Press the left controller secondary button to toggle the recipe panel.
- Use the reset control in the scene to clear active lab objects.

## Molecules Implemented
- Water (`H2O`)
- Hydrogen Gas (`H2`)
- Oxygen Gas (`O2`)
- Nitrogen Gas (`N2`)
- Ammonia (`NH3`)
- Carbon Dioxide (`CO2`)
- Methane (`CH4`)
- Carbon Monoxide (`CO`)
- Hydrogen Cyanide (`HCN`)
- Nitric Oxide (`NO`)
- Hydroxyl Radical (`OH`)
- Diazene (`N2H2`)
- Hydrazine (`N2H4`)
- Cyanogen (`C2N2`)
- Formaldehyde (`CH2O`)
- Methanol (`CH4O`)
- Urea (`CH4N2O`)
- Glycine (`C2H5NO2`)

## XR UI Notes
- Molecule library and recipe interfaces are designed as world-space UI.
- Molecule discovery items update visually when a valid molecule is formed.
- Recipe information can be toggled in VR using controller input.

## AI Tools Used
- ChatGPT for architecture support, XR debugging, and implementation guidance
- GitHub Copilot for code completion and boilerplate assistance
- AI assistance for project documentation

## Build Notes
- Android build target configured for Quest-compatible deployment
- OpenXR and Meta OpenXR packages included
- URP is enabled for rendering
- IL2CPP-compatible Android scripting backend is configured
