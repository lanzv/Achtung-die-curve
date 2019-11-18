# Achtung-die-curve
Final semestral program of first year studying
C # graphical application using Windows forms. The Achtung Die curve is similar to a classic snake, but with more snakes and all directions of movement.

This app is designed for 6 players (optional if the player is a human or computer).
Each player can choose the color, name and keyboard shortcuts of his snake. Then the game counts the score for each turn.

The AI algorithm is based on some basic heuristic and on a neural network with odd input neurons representing the pixels on the left side of the current snake position
and even input neurons representing the pixels on the right side of the current position. The smaller is the input neural index, the closer
is a pixel represented by that neural to current position of snake (using some BFSs). But there was a problem with teaching that neuron net, 
so it is not working as well as possible. 
