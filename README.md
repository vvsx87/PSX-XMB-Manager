# PSX XMB Manager
A new tool that simplifies the installation of homebrew and games on the XMB of the PSX.</br>
Now features also a backup manager for PS2 games on PC & PSX.

<img width="591" alt="Untitled" src="https://github.com/SvenGDK/PSX-XMB-Manager/assets/84620/82feae61-3cf9-44f2-b6f3-65d1789f9a80">

## Notes
- Requires FMCB installed on your memory card and Open PS2 Loader (to load games)
- Deleting a partition should be made with the latest wLaunchELF's HDDManager, however you can delete a partition in the Partition Manager.
- "XMB Tools" will be available on a later release.
- It is not recommended to abort an installation within the first 3%, this could corrupt your HDD. The same goes for the last percentages of the installation.
- Only connect your PSX's HDD locally if you know how to !

## Required drivers
NBD
- If you already installed the unsigned driver and used PFS BatchKit Manager before, then you can already proceed with creating a project.
- To connect to the NBD server of your PSX you will first need an NBD client and driver on your PC:
  - The Ceph MSI installer bundles a signed version of the WNBD driver. </br>
  It can be downloaded from here: https://cloudbase.it/ceph-for-windows/
  - Install the client and reboot (required)

Dokany
- https://github.com/dokan-dev/dokany/wiki/Installation

## Creating a new project
- Go to Projects -> New -> You can choose here between a game or homebrew (app) project
- Another window will open and ask for some details, please note:
  - Project name: REQUIRED
  - Save project at: REQUIRED
  - Game/Homebrew title: REQUIRED
  - You can load a game ID directly from the ISO file, just browse the ISO file before
- Save the project, now proceed to "Edit ressources"
  - Option 1: You can load your own cover art and screenshots by clicking on the picture boxes
  - Option 2: You can load all art from https://psxdatacenter.com/ by clicking on "Load from PSXDatacenter" </br>
  This works only for games and the game's ID has to be in the database.
  - When done, hit "Save ressources".

## Preparing a project
This process will add the latest OPL-Launcher as KELF to your game OR wrap the homebrew's ELF to EXECUTE.KELF.
- Select your created project from the list and simply hit "Prepare project"
- Note: You can also delete the "EXECUTE.KELF" file inside the "Tools" folder to download the latest version before the prepare process.

## Installing a project
- Launch Open PS2 Loader and the NBD server
- Connect to your PSX
- Select a prepared project from the list and hit "Install on PSX"
- After confirming with "yes" the installation process will begin
  - Note: If you install a game over NBD, this process can take up to 1-2 hours depending on game size. </br>
  Homebrew should be installed in some minutes.
  
## Installation Process Details
How a game will be installed when hitting "Install on PSX" :
- Installation with hdl_dump & -hide option
- Creation of the same partition as "PP" with pfsshell
- Modification of the partition header with hdl_dump
- Adding ressources with pfsshell

How a homebrew will be installed when hitting "Install on PSX" :
- Creation of a "PP" partition with pfsshell
- Modification of the partition header with hdl_dump
- Adding ressources with pfsshell

## Used tools from other developers
| Tool | Developer |
|-----|-----|
| hdl_dump | https://github.com/ps2homebrew/hdl-dump |
| pfsshell & pfsfuse | https://github.com/ps2homebrew/pfsshell |
| SCEDoormat_NoME | krHACKen |
