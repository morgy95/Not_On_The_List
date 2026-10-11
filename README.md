Not On The List

A top-down bouncer game made in Unity for the "Uninvited" game jam.

You run the door of a nightclub. Check every guest against the guest list, let in the ones who belong, turn away the ones who don't, and keep the club's money and reputation afloat. Let in the wrong people and the boss will fire you.

<!-- Add a screenshot or GIF here: ![Gameplay](docs/gameplay.gif) -->
Gameplay
Click a guest in the queue to read their ID card (first name, last name, age, job).
Open the guest list and compare. If the name is on it, Accept. If not, Refuse.
Accepting a guest who is on the list earns money and reputation. Accepting someone who isn't costs you both.
Rent and expenses are taken every month, and they grow over time.
If your money drops too low, you're fired: the game over screen shows your total money won and what you have left.

The full concept also includes kicking out drunk guests inside the club and closing windows before intruders climb in.

Features
Guest list and ID card system
Money, monthly expenses and a reputation star system
In-game clock (hour, day, month, year)
Main menu with how to play, volume setting and best score
Game over screen with money won and money left
Looping music that survives scene changes
Built with
Unity 6
C# and TextMeshPro
Music composed in FL Studio
Pixel art sprites
Run it
Clone the repository.
Open the project with Unity Hub (use the Unity version listed in ProjectSettings/ProjectVersion.txt).
Open the Menu scene and press Play.

A web build is available on itch.io: (add the link here)

Project structure
Assets/       scenes, scripts, sprites, audio
Packages/     Unity package manifest
ProjectSettings/
Author

Morgan Marcelin (@morgy95)

Made for the "Uninvited" game jam, 2026.
