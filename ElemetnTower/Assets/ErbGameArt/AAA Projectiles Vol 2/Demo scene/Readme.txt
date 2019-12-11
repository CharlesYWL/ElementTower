Asset Creator - Vladislav Horobets (ErbGameArt).
All that is in the folder "AAA Projectiles" can be used in commerce, even demo scene files.
-----------------------------------------------------

Using:

If you want to use post-effect like in the demo video:

1) Download unity free post effects 
https://assetstore.unity.com/packages/essentials/post-processing-stack-83912
2) Add "PostProcessingBehaviour.cs" on main Camera.
3) Set the "Post-effects" profile. ("\Assets\ErbGameArt\Demo scene\CC.asset")
4) You should turn on "HDR" on main camera for correct post-effects. (bloom post-effect works correctly only with HDR)
If you have forward rendering path (by default in Unity), you need disable antialiasing "edit->project settings->quality->antialiasing"
or turn of "MSAA" on main camera, because HDR does not works with msaa. If you want to use HDR and MSAA then use "MSAA of post effect". 
It's faster then default MSAA and have the same quality.


1) Shaders
1.1)The "Use depth" on the material from the custom shaders is the Soft Particle Factor.
1.2)Use "Center glow"[MaterialToggle] only with particle system. This option is used to darken the main texture with a white texture (white is visible, black is invisible).
    If you turn on this feature, you need to use "Custom vertex stream" (Uv0.Custom.xy) in tab "Render". And don't forget to use "Custom data" parameters in your PS.
1.3)The distortion shader only works with standard rendering. Delete (if exist) distortion particles from effects if you use LWRP or HDRP!
1.4)You can change the cutoff in all shaders (except Add_CenterGlow and Blend_CenterGlow ) using (Uv0.Custom.xy) in particle system.

2)Light
2.1)You can disable light in the main effect component (delete light and disable light in PS). 
    Light strongly loads the game if you don't use light probes or something else.

3)Scripts
3.1)"AutoDestroyPS" is needed to destroy parts of the effects through a lifetime in the particle system.
    If you want projectiles to exist on the scene longer - just increase the Duration and Lifetime!
3.2)"ProjectileMover" is created for demonstration purposes only. The main product that you buy is effects.
    Use: Projectiles fly out with a “Fire Point” that you need to select on stage.
    The angle at which projectiles take off depends/isTheSame as on the object on which the script hangs.

4)How to modify the existing prefabs
4.1)If you reduce projectile speed, you also need to find the “Trail” tab in the particle system and increase the trail's lifetime.
    You also need to increase the Duration and Lifetime in all components with a particle system.
    When increasing speed, do the opposite.
4.2)When resizing projectiles, you need to change the value Emission> rate over distance if it exists in one of the components.
    If you double the size, you need to halve the "rate over distance" value.
    When reducing the size, do the opposite!
4.3)All Hits and Flashes can be resized using "transform" in the main component.
4.4)Tutorial how to make target projectile: https://www.youtube.com/watch?v=LJLWNnqAjQ4

5)Linear color space + HDRP
5.1)Dissable soft particles or uncheck checkbutton "Depth" in all materials if you use HDRP.
5.2)Reduce "Mask clip value" in all materials from Blend_TwoSides" shader if you use Linear color space.
    You can find all materials by searching "ts" and "twoSides"

Contact me if you have any questions, ideas or suggestions.
My email: gorobecn2@gmail.com


PS. Do not forget to rate this asset, this will greatly help ^^