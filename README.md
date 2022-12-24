# Simple Mechanics For Unity3D
Here I load my simple mechanics like CharacterControllers, CarsControllers, WeaponsShoots and other things.


### Player Controller (User's Manual)
You just need:
- Empty Object: with Character Controller, Player Input and PlayerController script.
  - PlayerCamera: You will put this camera in PlayerController script
  - PlayerObject

![PlayerController Screenshot](https://raw.githubusercontent.com/wikicode96/simple-mechanics-for-unity3d/main/Screenshots/PlayerController.jpg)

### Car Controller (User's Manual)
For make a car you need a specific hierarchy (Example in the screenshot):
- Car: Whit Rigidbody (1000kg of Mass for example), CarController script (500 Max Torque each and 30 Max Steering Angle for example)
  - MeshCollider
  - WheelsColliders
    - FLWheelCollider
      - FLWheelMesh
    - FRWheelCollider
      - FRWheelMesh
    - BLWheelCollider
      - BLWheelMesh
    - BRWheelCollider
      - BRWheelMesh
      
![CarController Screenshot](https://raw.githubusercontent.com/wikicode96/simple-mechanics-for-unity3d/main/Screenshots/CarController.jpg)
