**Word Count**<br/>
https://github.com/LuminosoInsight/code-sample-term-counting<br/><br/>

**Description**<br/>
A word counting application for a Luminoso job application test.<br/><br/>

This application requires a text file as input and will produce the complete count of words used in the text file.<br/><br/>

**Version of Python Used**<br/>
Python 3.6.3 :: Anaconda, Inc.<br/><br/>

**Running**<br/>
python word_count [-hj] [\<inputfile><br/\>]
- -h help<br/>
- -j output in JSON<br/>

if the optional \<inputfile\> is not specified, then stdin will be read<br/><br/>

There are a few sample/test files included. The main one used for testing was the Gettysburg address in the file gb.txt. Other longer samples are _The Federalist Papers fp.txt_ and _Don Quioxte dq.txt_ A sample command line to run this application is:<br/>

> python wordcount.py -j gb.txt<br/><br/>

**GIT**<br/>
The application build includes a git repository of checkins as the application was being built. Use 'git log' to see the history.<br/><br/>

**Unit Testing**
The application includes the class TestWordCount.py which is a PyUnit class to test the application. To test on the command line use:<br/>
> python TestWordCount.py<br/><br/>

I've never used PyUnit before. I've written a lot of JUnit tests in the past and PyUnit seemed very similar. This is not uncommon for me to learn as I go and this is a perfect example that I thought I should point out.<br/><br/>

**Design Considerations**<br/>
I considered making the application object oriented, but the app being more of a one off bash style script (in python), the object oriented vs. simple functions seemed over kill. It could have gone either way. I chose simple functions. Consider it an early design choice.<br/><br/>
Removal of punctuation is a tricky topic and I went back and forther on removing hyphens. I chose to not remove hyphens because the text files from Project Gutenberg seemed to not hyphenate words acroos lines, but there are some properly hyphenated words like Guteberg-tm isn't a proper word it seems clear tha the project would want the -tm included, although this brings up another case where the word should probably be "Project Gutenberg-tm" which could be a further enhancement to have some full names as singluar items instead of splitting them.


**Enhancements**<br/>
I would consider the following enhancements
1. A file containing a list of non-words
1. A file containing a list of words that were similar and should be counted together
1. A file containing a list of words that are too common to count and maybe a option to toggle on/off
1. The application builds a dictionary then converts to a series. It might be more performant to just start with a series.

