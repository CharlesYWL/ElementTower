# ElementTower Game Basic Information
A Tower Defence game combined with AutoChess.    
![](./Src/ETLOGO.png)
## Summary
### Our Inspriation
Classic Tower Defence Game: Kingdom Rush.    
![](./Src/KingdomRush.jpg)
New trending AutoChass game: DotA2 AutoChess.    
![](./Src/dotachess.jpg)
We want to combine Classic Strategy Game Tower Defence and AutoChess with some interseting gambling system like drawing cards from a card pool.
### Background Story 
Long long ago, this world has been controlled by a cruel devil. However, a group of warriors came here and defeated this devil to free the people on this land. The devil has been keeping developing his power to try to come back one day. Now, the devil is back, and warriors are leading to fight to protect the land.
### Basic 
We have designed eleven elements towers in total. They are, crystal tower, infernal tower(namely fire element), shadow tower, 
### Gameplay explanation
You will need to build your element towers to defeat the enemy waves and protect home. There are totally three Gates from the wall that enemy broke, and they will flood in through three routes. Fortunately, the Enemy will only start their attack during Night time. So, arm yourself and build the element towers during Day time.

There are a total of 11 different element towers with 5 different rarity.Their rarity will be changing along with the game progress and waves. With two identical towers in your deck and one on the field, you can consume the 2 identical towers in your deck and upgrade the tower on the field. Max level of each tower is Lv. 3.

You can also re-collect the towers on the field into your deck and even sell them to the store. Every time you have extra money, you can refresh the store to get another 5 new towers and buy them to arm yourself up.

There will be totally 10 waves, with the last wave being the Boss Battle. Build your front so that you can defeat the Boss and save the people behind the wall!

## Game Mechnism
### Economy system
![](./Src/Toppanel.png)    
We reward players coins by elimanating enemies, and players can utilze those coins to buy more towers and upgrade them. But the probiliy of drawing cards depands on their rarity. Rare cards will have lower probiliy.    
![](./Src/Upgrade.png)
### Combinaion between different towers
Like Combinaion in AutoChess, we also have insane effect when certain towers exists on the stages.    
![](./Src/IceOcean.png)    
![](./Src/Combo2.png)
## Main Roles
Fengqiao Yang - Map Design, Camera Movement & Animation
Annie Qin -- Enemy Movement, Tower Animations, Health Bar Control and animation 
Weili Yi -- Game Logic: card pool system, User Interface
Zijian He -- Game Logic: combination of elements, damage engine, Movement/Physics
Zhongquan Chen -- User Interface
	There are two parts  in Map Design: Battle Field and Background Terrain. For the Battle Field, I used the online resource of ground, water, walls, bridges and trees and arranged them into the scene. For the Background Terrain, I was using the Terrain Tool in the Unity Asset Store to paint out the Terrain with different brushes and effects.
	The beginning camera was designed and implemented by using the Animator that was built in the unity. Considering we have a relatively large map, we decided to have a MOBA-like camera and I attached a script for the camera movement to our main camera.
	Most of the Animation I did was in the tower prefab. The major implementation method is using the animator and the animation controller to switch between different animations. The VFX effect of Lv. 2 & 3 tower is from the RPG VFX Pack, and we combined the magic spells to our Lv2 & 3 tower prefabs.
### User Interface (*ZhongQuan Chen, Weili Yin*)
For user interface we decide to put into 3 scenes, one is start scene, where the user is allows to change the volume setting; one game scene, which namely is a game scene to play; and one ending scene, which will display all the credits for this project. 

**Start Scene - Main menu scene**
The start scene is where players will see in the first place. In this scene, we designed some buttons for user to interact with. Say, we have play button, which will load the next scene in queue, also known as game play scene. We have a setting button where players are allowed to adjust the volume when they enter the game. To adjust the volume in the game, we made an AudioManager game object which will adjust the volume for game music and also for game play music (tower attack sound). At last we have an exit button, which it will exit the game.

**Game Scene - Main playing scene**
In the scene, the user interface will be players information, towers’ information and also choose tower interface (shop, refresh, etc)

We display players’ information at the top of the screen. In the top panel(namely player info display), we will keep count of players’ money during the game. This will involve in money system in the game, which will be discussed in the following section. There are also stage display to show whether player should build tower or should prepare for fight. Additionally, players will notice there are two buttons on the corner of top panel(namely player info display). The two buttons will be the main playing button where players will be able to open shops and refresh the shop to draw towers during the game. On the left hand side of the game scene, there is also a setting button, where players are freely to choose to exit game or adjust volume. 

**Game Scene - Shop (Yin weili)**

*Money System*
Money system is one important system in the game. Players in the game will need money to build towers to define their castle. The money system involves two major parts, one is spending money on building towers; one is selling towers to earn money or killing enemy to earn money.

*Money System - Spending Money*
Players should spend their money wisely, because if players spend them quickly, they would not hold the castle later in the game. So, spending money becomes a tactical decision that players should take into consideration. There are two ways of spending money. One is to buy towers from the shop, another is to refresh the shop to get new towers. Unfortunately, in the beginning player will have to spend money in order to get towers to defend. We want player start up difficult so that they will know each refresh button they clicked in the game, will take a major part of win or loss. (We have a cheat mode that players can spend as many as they want, but that will be another case if they can find them in the game). Buying tower also be important. Players have to consider what tower they have for now and what kind of combination they choose to use in the game. Tower combination, and upgrading tower level, etc. these are the facts that players should buy tower wisely. 

*Money System - Earning*
One money income source will be killing monsters. Players build up tower to kill monsters, so that they earn more to build more towers and kill more monster… 
Ending Scene
Ending scene will be some credit to our game project. We are a great team, and we are proud of being part of this project. Therefore, credit scene is the most significant one.

### Movement/Physics
**Movement** (*Zijian He*)
The implementation of the movement design basically focuses on the enemy movement and the projectile of each tower. We used the combination of Unity 3D mesh system and animation to implement the movement of the enemies in our game. The choice of rigidbody is not in our concern due to the mesh system and the automated system. For our enemy movements, we have four levels attached to how hard and strong the enemy is in our game. Below is our weakest enemy, as a level 1 enemy, we set the health of it below average to around 50 health. 

For the projectile movement, we tie each projectile to each single enemy, and the projectile movement significantly depends on the speed  and the position of the enemy. The collision system is another huge way we build our relation between towers and enemies. This allowed the game to almost automatically implement every movement so that the player can focus more on his strategy to play through the game.

**Physics** (*Zijian He*)
Colliders - The projectiles from towers and everything they interact with has an associated collider. Towers collision will prevent the enemies get through the tower prefab and tower overlapping so that the player can easily set the tower as much as possible. Each enemy has its own collision so that the tower can detect where the enemies go. 
The ground and walls all act as regular colliders and three gates represent the spawning points of the enemies. With the help of wave points on the map, enemies will know where they can move during the game. 
Damage Engine: With different types and rarity of towers, the damage engine is different. While the level 1 tower should have least damages causing on the enemies, and the level easy type of enemies should also have the least amount of health so that player can get better gameplay experience in the beginning. Moreover, when the stage gets bigger, the enemies should be stronger and the health of enemies also depending on the sizes of enemies. We design the last stage, “Boss” come out, the rotation of the enemies will have 4:1 ratio compared to the normal enemies before, in the meantime, the speed of “Boss” will also be decreased so that it will give the player a feeling of how hard they need to defend on the last stage.

### Animation and Visuals
Annie Qin, Yangfeng Qiao 
List your assets including their sources, and licenses.
Carboard box, alcohol bottle ; CC3.0-by 3.0 license, Author - Clint Bellanger
Player (man with pony tail) ; CC3.0-by 3.0 license, Author - Hugues Laborde
Shadow character ; CC3.0-by 3.0 license, Author - Hugues Laborde
Cigarette ; CC3.0-by 3.0 license, Author - OceansDream
Wall and Ground Tiles ; CC0 license
Describe how your work intersects with game feel, graphic design, and world-building. Include your visual style guide if one exists. For the animations, I created many more than we ended up using based on the narrative and the final abilities for the player, but I primarily built animations for the player and the shadow (enemy) character. To avoid having to make my own pixel art, I chose to find full packages of pre-made art assets that I could divide up into the various individual animations and create a state machine for them.
Our initial visual idea for the show was to create a scene that resembled the location of a film noir movie. Therefore, we agreed upon a night city background, which perfectly set the mood for our narrative, where our player struggles with cigarettes and alcohol and is trying to get to rehab.

**Player and Enemy Character**
Our player has a third-person view of the whole map and the world, and he/she needs to choose different towers to try to beat the enemies. So basically our player’s animation are the tower’s animation. In total we have 11 elements towers, and each of them has three levels, so in total, we have 33 towers. Each elements tower will have their own type of crystal, and we achieve this goal by building different material of the tower crystal and put an animation asset on the crystal to help user identify which type of tower it is. By upgrading the tower to the next level, we have different size and type of magical circle under the tower. Second level have two magical circle, third level has three magical circle with different size. We also have different animation for the crystal. There are two animation property of the tower crystal, one is when it is idle and the other one is when it is attacking. When the tower is attacking, the crystal is rotating, and when the tower is idle, the crystal constantly move up and down.

For the enemy animation, as we said in class, we set different way point to show the enemy animation. For enemies, we have a total of five types of enemies, each enemies have two animation sets, one is when they are in movement, the other is when they die. So we set different way point to show different movement of the enemy. 

**Camera**
Yangfeng Qiao 

### Input 
Weili Yin:
UnityEngine.EventSystem: in our most Scene, instead of normal button click, we handle our most click,drag,drop thing through EventSystem like IpointerClicHandler, IBeginDragHandler and IEndDragHandler. This is because simply button click cannot handle the information on the thing we are dragging. By using OnDrag(PointerEventData eventData), we can implement drag cards to some point and create a new tower.
Mouse: We use the most intuitive way of controlling, dragging thing. Players can buy cards in shop and in hands, they can drag them to build point to create them.But if given more time, we want to achieve the dragging way like WarCarft3, build towers in any place and when you drag things on the map, you can see how the builds will be in the scene. 
ShortCut: Because our targets are those who play either AutoChess or Tower Defense, we inherit the shortcut from autochess. R for refresh shop, TAB for open/close Shop, ECS for pause and option menu.

### Game Logic

## Fun points
Like all TD game, we want players enjoy the combination they created to pass the game and also enjoy the random drawing system to make game more challenging.
