# Card.Net

Simple card engine for creating simple apps.

#### Summary

It was first targeted to .NetCore; but since it was too time consuming, I decided to make it .Net Framework 4+ for now.
I just wanted to try to make some kind of engine that you can edit some stuff with some scripts and
run it afterward in a controlled environment. I made a console app first but my goal is to use WinForm or WPF
as an editor.

## Requisites

- Visual Studio 2019 with latest framework

## Ideas

- Layers should be done at application level using panels. Simplify the whole engine process.
- Make a global space where you can write functions and or class
- have database like capabilities (different sources: csv, mySql, url with json, etc)
- have strings per language. (string tables...)
- have concepts of template page (that can be added as external layer)
- Instanciate a new sandbox for each page - using global scripts and other variables.
- Could compile in background and have the list of available methods for auto-complete.

## Todos

- Include GlobalScripts in the code generation. Don't know how I should handle that... Maybe globalScriptName.blabla like objects.
- Add string symbols that can be replaced with a global table that can be in multiple language. {{symbol}} with regex
- Not sure if I should remove the renderer - Since the render is done on the other side...
- Auto-complete (get variable names, keywords and api functions)
- Be able to deploy and generate.

## Bam: Basic Asset Manager
* Have different modes: prod, bundle, zip with array...
* In dev, use local or remote links, in prod, bundle it with the code.
* Should be updatable, so each asset version should have a version (using some kind of CRC)
* Might be good that the updater part is managed by it.
* Should be some kind of virtual file system.
* Type of resources: 
  * string
  * object
  * binary