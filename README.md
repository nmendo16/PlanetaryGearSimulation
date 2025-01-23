<h1> A Simple Planetary Gearset Interactive Simulator</h1>
by Juan Pablo Amorocho and the OER Lab for Centennial College.

This simulates the physics of a planteray gearset with adjustable properties such as:
<ul>
  <li>Number of cogs in the sun and planet gears.</li>
  <li>Speed and torque of each element.</li>
  <li>Lock and unlock the ring gear for different movement.</li>
</ul>

Controls:
<ul>
  <li>A & D or <- & -> Arrows to rotate the gearset counter-clockwise or clockwise respectively.</li>
  <li>Q & E to rotate the whole gearset.</li>
  <li>Mousewheel to zoom in and out. </li>
</ul>

<h2>How to Build</h2>
Due to the properties of Github Pages, we need to take into account the following:
<ul>
  <li>Build for WebGL in Unity. Make sure the Quality is set to Ultra, as this one includes the outline shaders whenever a gear is selected. </li>
  <li>The current build folder is a_build. Whenever you're ready to publish for Github pages, transfer the <b>contents</b> of this folder to the root location and replace any old builds already located here.</li>
  <li>The contents inside the a_build folder shoulder be two folders, TemplateData and Build and an index.html file. </li>
</ul>

<h2>Deployment</h2>
The current deployed version of this fork is at: https://jpablo9988.github.io/PlanetaryGearSimulation/ </br>
As this is a Centennial College OER project, feel free to visit that fork for the most up-to-date version of the project, which can be seen here: TBD.
