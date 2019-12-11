Asset Creator - Vladislav Horobets (ErbGameArt).
All that is in the folder "AOE Magic spells Vol.1" can be used in commerce, even demo scene files.
-----------------------------------------------------

If you want to use post-effect like in the demo video:

1) Download unity free post effects 
https://assetstore.unity.com/packages/essentials/post-processing-stack-83912
2) Add "PostProcessingBehaviour.cs" on main Camera.
3) Set the "Post-effects" profile. ("\Assets\ErbGameArt\Demo scene\CC.asset")
4) You should turn on "HDR" on main camera for correct post-effects. (bloom post-effect works correctly only with HDR)
If you have forward rendering path (by default in Unity), you need disable antialiasing "edit->project settings->quality->antialiasing"
or turn of "MSAA" on main camera, because HDR does not works with msaa. If you want to use HDR and MSAA then use "MSAA of post effect". 
It's faster then default MSAA and have the same quality.


Using:

1) Shaders
1.1)The "Use depth" on the material from the custom shaders is the Soft Particle Factor.
1.2)Use "Center glow"[MaterialToggle] only with particle system. This option is used to darken the main texture with a white texture (white is visible, black is invisible).
    If you turn on this feature, you need to use "Custom vertex stream" (Uv0.Custom.xy) in tab "Render". And don't forget to use "Custom data" parameters in your PS.
1.3)The distortion shader only works with standard rendering. Delete (if exist) distortion particles from effects if you use LWRP or HDRP!
1.4)You can change the cutoff in all shaders (except Add_CenterGlow and Blend_CenterGlow ) using (Uv0.Custom.xy) in particle system.
1.5)Lit_CenterGlow shader. It's a simple opaque shader with emission.
1.6)Blend_LitGlow shader. The same shader as a blend shader, but with lit glowing. The shader takes light from all lights from the scene.

2)Light.
2.1)You can disable light in the main effect component (delete light and disable light in PS). 
    Light strongly loads the game if you don't use light probes or something else.


Contact me if you have any questions, ideas or suggestions.
My email: gorobecn2@gmail.com