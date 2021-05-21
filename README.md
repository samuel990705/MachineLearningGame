# MachineLearningGame

![Commit History] (./img/commitHistory.jpg)

Controls
- W,S to accelerate/decelerate
- A,D to steer left/right
- SPACE to brake and drift

# Features
WheelColliders
- WheelColliders were used to simulate physics of driving
- Vehicles are all-wheel drive (all wheels are powered)
- Drifting was implemented by changing sideway friction of rear wheels such that they glide when brake is held

Mischellaenous
- An autoflip script is added to set vehicle upright if vehicle is flipped for longer than 3 seconds. 

# Machine Learning

ML Observations (15 inputs total)
- own position (3 inputs)
- own rotation (3 inputs: euler angles rather than quaternion) 
- opponent position (3 inputs)
- opponent rotation (3 inputs)
- ball position (3 inputs)

ML actions (vector2)
- two Vector3 inputs: one for steering one for acceleartion
- previously had bakes, but removed to simplify ML model
- discrete (steer left/right and accelerate/deccelerate)

ML OnEpisodeBegin (start of a new iteration of training)
- randomize location of vehicles to better generalize agent

ML reward design
- Penalty for existing (used to speed up training), encourages movement
- Penalty for colliding with other player
- Penalty for colliding against wall
- Add reward for touching ball
- Add reward for scoring 


#Proof-Of-Concept Simplification
Since training would have taken way too long for wheelcolliders, a Proof-Of-Concept scenario was created to demonstrate the model. The following simpifications were made to the original game...
- Reduced to 6 inputs: removed rotation of players and y readings (from 15 inputs)
- Reduced to 6 outputs: forward/stay/backward and left/straight/right
- Football field was drastically reduced in size (to increase likelihood of agent scoring)
- Vehicles were replaced with cubes controlled by Rigidbodies with rotation on y-axis locked

