# Multiboard Tetris

The core gameplay innovation I want to try first in the theme of `Innovate on Tetris` is a "Multiboard Tetris" game. Instead of having one board, have multiple boards stitched together. As you complete some objective on the first board, the camera moves to the next board (which is connected to the first board). 

As an example, where the first board would be something like a standard Tetris board (w:10, l: 20), the next board would be wider than long. And instead of the new piece dropping from top to bottom, it would spawn on the left and "drop" to the right. 

This means there's overlap between where one board ends, and another board begins. Which means if you have standard Tetris rules- create lines at the bottom- then you're going to have an issue where the blocks from the previous board are stuck at the spawn of the new board. 

To get around this, I figure we have to change the ruleset of Tetris- no stacking blocks at the bottom and clearing as many lines as possible before losing. Instead, we'll present a board _with pieces already placed_ with the objective being to clear the pieces or clear X number of lines or w/e. 

This is why we needed the BoardEditor- a way to build and save boards that had pieces already drawn. 

The next thing we need to do is build out a system that manages the gameplay of multiple boards.

## Gameplay Manager

As it sounds, the intent of the Gameplay Manager is to manage the gameplay of multiple Tetris boards. Each board can have unique rulesets and objectives, all defined in a Gameplay Controller. We need a way to stitch these boards together and have a manager that says which board is active, and when. This is the intent of the Gameplay Manager 

### Details

- Has multiple Gameplay Controllers
    - Gameplay Controller has one Board
- Gameplay Controllers have mandatory methods
    - Initialize
    - GameStart
    - GameEnd
    - Terminate
    - Interrupt
- Manager is the caller for each controller method
  - Likely through events
- Controllers issue events to Manager as well
  - Game is started
  - Game is ended
  - Update (send some data back to the Manager that the manager might want to know about- think a individual board score than might feed into a cumulative score)
  - Game is interrupted
    - Game has resumed?

### Lifecycle 

- Init Self
- Init `Game Controllers`
- Setup Listeners to GC children
- Instrument Listeners to children
- Wait for all GC init events 
- Order games / setup game session
- Start first game 
- Listen for game finish event
- Trigger next until all games complete
- Send terminate to all GC
- Trigger "finish session" event

### Event Driven Management

My thoughts are to take my learnings of events and bring them into gameplay management. The requirements here is that we need more than one board, and a way to manage which board is "active". 

So what I want to try is writing an `interface` that define how the Game Manager is going to interact with its controllers. The manager will trigger events that the controllers listen to. These events mainly deal with the games state (might want to setup an actual `state machine` here).

I like the idea of an interface because it means each controller gets a ton of flexibility. The ruleset might be incredibly different from one controller to another, and I don't want the manager to care- If you are going to be managed by a Gameplay Manager, you will send/receive certain events types. An interface accomplishes this quite well. 

### Gameplay Ordering

The thing I need to consider now is how to order the games. If we have 3 boards in our gameplay session, how does the manager know which board is first, second, third? 

The answer seems to be some piece of data in the game manager with the desired order. An ordered list of GC ID's. If I'm going to trigger "GameStart" via event, I need to send the controller ID as a parameter for this event- all the controllers will receive this event, so I need to specify which controller I actually want to start. 

### Gameplay Session Serialization

So we are defining a "Gameplay Session" as the beginning of the first controller GameStart event to the end of the last controllers GameEnd event- basically a session is the gameplay managers lifecycle. 

We've also spelled out that there is going to be some data associated with the Manager- at the very least we'll need a set of Gameplay Controllers and their order in the session. 

It would make sense that we want to Serialize a gameplay session- once we have designed a fun multiboard tetris layout, we'd want to save that for later- hence serialization.

This means we'll need the following Serializable Objects:

#### Gameplay Manager

- A ordered set of references to gameplay Controller object

#### Gameplay Controller

- A controller ID for the Gameplay Manager to reference  
- A reference to a BoardData object 
- Probably some ruleset data object
- Probably a set of "gameplay objects"

#### Ruleset

- ActivePiece enabled? Drop direction (top -> bottom vs left -> right)
  - If the Ruleset is enabling ActivePiece, then it might make sense for the set of gameplay objects to live within the Ruleset, not directly into the Controller.

#### Gameplay Objects

- ActivePiece is an example of a gameplay object
- We would need to store the data of ActivePiece somewhere (right now its hardcoded, which may be fine for now)
- I we wanted to extend the types of pieces beyond the standard set of Tetrominos, then we would likely want to pull that into a true serializable object.
  - Think of adding Bombs, special pieces, individual blocks, etc.

### Technical Debt Rears It's Ugly Head

And before I can do anything, I must remember that I left the current Gameplay Controller in a bit of a state. I kept feeling like I had something conceptually backwards with the BoardManager/Tester, and I finally realize what it was- I had inverted the ownership of objects. 

My refactor of "Board does everything" to this concept of a "Gameplay controller" still had the Board being the thing I was instantiating. I then had a ton of trouble trying to cram in a Gameplay Controller into the BoardFactory. This was the inversion of control- the GC should be my entrypoint. A gamecontroller HAS a Board- not a Board has a GameController. 

So I need to correct the object control here, then also hit that TODO of pulling the "ActivePiece" back into its own object, THEN I can actually start building out the Gameplay Manager.