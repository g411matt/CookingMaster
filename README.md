# CookingMaster
Cooking Master Prototype

Controls:
Player 1:
WASD move
E interact
Escape pause

Player 2:
Arrow keys move
ctrl interact

All design specifications have been met

The only randomness in the prototype is pickup spawning and customer orders, everything else operates on fixed timers/scores, which is something that would change in the future, for instance different types of customers wait different lengths of time

The vegetables are generically given letters instead of names and the customer orders use random vegetables, which is fine for prototyping, in the future with real vegetable representation customer orders would need more structure, a base leafy vegetable plus toppings

Pitting players against each other with separate scores and timers felt like it doesn't work with the kitchen co-op aesthetic, you need teamwork to get orders out efficiently, but that dissolves when players fight over who delivers the order

The GameManager script wound up handling too much in the end I would want to divide it into at least 2 managers in the future 1 for ui the other for game logic

Pickups and vegetables should get set up with object pools for performance when not using programmer art

Would probably want to set up ScriptableObjects to hold a lot of the adjustable data, like score values, times, order recipes, etc.

The input system would need proper building with rebindable keys and gamepad support
