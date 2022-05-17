# MAARS (Multi-agent Augmented Reality-based Simulator)

MAARS: Multi-agent Augmented Reality-based Simulator

# Installation Instructions

 - Clone this repository to your machine `git clone https://github.com/bu-air-lab/maars.git`
 - Install **Unity Hub**, which you can download [here](https://unity.com/download).
 - Install the proper version of **Unity Editor** (currently **2020.2.7**), which can be downloaded [here](https://unity3d.com/get-unity/download/archive).
 - Open **Unity Hub**, and in the Projects tab use the drop-down attached to the open button to select *Add project from disk*.
 - Navigate to your local repository and select the **MAARS** directory within. Select *Add Project*.
 - Back in **Unity Hub** you should be able to see the **MAARS** project in the Projects tab. Select **MAARS** and open the project!
	 - This may take a while, especially on your first time opening the project.

# Running Sample Environments in MAARS
Included with this repository are multiple sample environments that show the kinds of applications MAARS can help simulate. These environments are each individual scenes included with the Unity project.

 - Select an enviroment from within **Unity Editor** by using the *File* tab, selecting *Open Scene*, navigating to the *Assets/Scenes* directory within MAARS, and selecting the scene you would like to open.

# Sample Environments in MAARS
## Office
The Office environment is kept in *OfficeScene.unity* and models an office where a human worker works alongside a team of robots, in a shared office environment. The human worker can monitor the robots that are working alongside in the shared environment.

 - There is a tablet on the worker's desk that enables the human to track the status of the robots, including their current location, and their future plans.
 
## Shopping
The Shopping environment is kept in *BUScene.unity* and models a small shopping mall where the user can remotely make purchases as multiple vendors.

 - The human's view is on display 4 in the Unity Editor here.
 - In the top-left corner of the human's screen there is a drop-down bar where the human can select a product to "buy". Choosing a product will point the human's camera to that product's vendor.
 - Clicking the "buy" button will send an employee with the product from the vendor to the human. The employee's path can be seen highlighted on the screen.

## Warehouse
The Warehouse environment is kept in *Warehouse.unity* and models a simple warehouse with rows and columns of racks, a human, package drop stations, and package-collecting robots.

 - Included is an AIHuman that will automatically travel between drop stations to help robots, as well as a KeyboardControlledHuman that can be enabled to allow the user to move the human manually with WASD or the Arrow Keys, and the Mouse to look.
 - The number of rows and columns of racks, number of robots, and other variables can be modified withing the WarehouseCreater GameObject to scale the warehouse to the required size.


## Todo

- Check if the readme works for both Ubuntu and Windows (like git clone)
- Steps to show how to change the display from 1 to 4 for the shopping environment.
- Steps to show how to change back to display 1 for other environments.
