# Live Caption Translator

This is a UiPath Custom Activity.
Voice recognition and translation are performed in real time, and the results can be displayed on the screen as subtitles.

## Package Description

This package provide voice recognition and translation by using IBM Watson SpeechToText and LanguageTranslator Service.

send PC speaker output to IBM Watson cognitive service, get voice recognition result as text in real time. and you can choose tranlate or not as option. 
and also export file to take the minites  book as option.

Due to the self-restraint by COVID-19, I have developed it so that you can freely and comfortably watch video content and Webinar in various languages around the world.
Therefore, it does not recognize the microphone input, only the speaker output.

This package has 3 activities.

Streaming
 - Speech To Text Streaming Scope
 - Start Streaming Recognize
 - Stop Streaming Recognize

## Advantage

Supports any web conference tool including Microsoft Teams, Skype, Zoom and video content like youtube, because of capture PC speaker output.

if you register IBM Cloud lite account for free, 
you can use Speech To Text Service 500min per month, 
Language Translator can translate up to 1,000,000 letter per month.

## compatibility

UiPath Studio 2018.4+

## Dependency

IBM.Watson.LanguageTranslator.v3 >=4.3.1
IBM.Watson.SpeechToText.v1 >=4.3.1
NAudio >=1.10.0
