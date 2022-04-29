## Alpha4 Retrospective

I really liked the progress and learnings I got from building out the BoardEditor. 

Last time I had done something like this was in 2016/17. Unity had a lot less support for 2D games- no native Grid system, legacy UI system was much less supportive. I remember I had to do a lot more internal plumbing to do things like have a Grid, and weeks were spent getting a reasonable Canvas-to-world-space system running. 

The BoardEditor, in just over a week, exceeds that 2016 prototype- which took months. It's now fully functional (if ugly) and enables me to work on the actual gameplay prototype I wanted to work on. 

### Biggest Learnings

- Unity native UI system
- C# events - Unity's UI system pairs well with an event system. Events make it relatively easy to reason about the flow of your application while maintaining decent de-coupling of individual elements- especially in the case where one event can affect multiple elements and states.

## Next  Steps

### Marketing
- Build for Web 
- Automated build per release 
- Generative Blog 
- YouTube channel setup and DevLog release

### Prototype

#### Scene manager

- We want a single entry point for the game
- Splash Screen
- Title Screen 
- Main Menu UI 
  - Sends to Board Editor
  - Sends Board Tester/gameplay/etc.

#### Input System Overhaul

- Use new Unity input system mapping 
- Overhaul current implementation to be Web / Mobile native

#### C# gRPC server 

- Move from local file store to a server prototype 
- We'll want to keep the local file store for cachable items
- We'd want a server for any trusted resources 

#### Multiboard Tetris 

- The actual gameplay prototype
- This is probably a great place to revisit
- Forces re-refactor of Gameplay controllers
  - ActivePiece gets its scope back 
  - Add support for multiple kinds of gameplay controllers
  - Essentially give us the ability to define multiple rulesets for Tetris
- Allows us to further reduce scope of Board, which should do nothing but handle the visual elements + pure mechanics of a Board (in bounds, etc.)
- Each board has a gameplay controller

### Stack Ranking

Listing the things that are top of mind, by far and away the most important thing for me at the moment is Multiboard Tetris.

However, I am wondering if its better to stand up build systems- and more importantly CICD system- earlier than later. The thinking here is that a build is harder later when the game is more complex. 

Additionally, my fear/thought is that the build side is more opaque and dragon-infested. In this case, setting up earlier is better- and then iterative fixes per release as opposed to a massive headache trying to build a "huge" game.

#### Feature Ranking

1. Multiboard Tetris
2. Build for web 
8. Automated Builds
3. Scene Manager
4. Generative Blog
5. Input System Overhaul
6. Build for Mobile
7. gRPC server