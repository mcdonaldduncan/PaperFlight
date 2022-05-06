SETUP:
1) Drag the “Northern Lights Parent” prefab into your scene
1) Place one or more prefabs into your scene as children of the northern lights parent
2) Position the northern lights prefabs until the desired effect is reached
3) Assign the "target" variable of the northern lights parent object via FollowTarget.cs. The target should be the player/camera transform

Because the northern lights should essentially be part of the skybox and be treated as distant objects, I recommend that you place each of the northern 
prefabs into a specific non-default layer(such as TransparentFX) and use a secondary camera to render them. 
To accomplish this:
1) The main camera in the scene should be modified with the following settings:
	•	Clear Flags: Depth Only
	•	Culling Mask: All relevant layers except for the layer the northern lights prefabs are set to
	•	Depth: 1
2) The secondary camera should be a child of your main camera and should have the following properties:
	•	Clear Flags: Skybox
	•	Culling Mask: Only the layer that the northern lights prefabs are set to
	•	Depth: -1
	•	Set far clipping plane large enough to avoid clipping issues

This camera setup is not required, however, to give the northern lights an appearance of being the proper scale while avoiding issues with camera clipping, I would recommend it. 
The demo scene shows an example of the above camera setup 




CUSTOMIZATION:
Colors: For the particle based prefabs, the color is most easily changed by changing the "start color" property. 
For the "curtain" effects, simply change the color of the material assigned to the mesh.

Sky Coverage: To change what percentage of the sky that the northern lights are seen, the Y property of the 3D start rotation can be changed. If
set to 360 degrees, then northern lights will be seen all around the player. At 180 degrees, half the sky will have northern lights

Density: To increase the amount of northern lights in the particle based prefabs, simply change the "rate over time" property in the "Emission" tab of the emitter. 