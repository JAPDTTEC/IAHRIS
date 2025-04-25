'Option Compare Database
Option Explicit On

Namespace Ayuda
    Public Class Ayuda

        Public Const HELP_CONTEXT = &H1         'Display topic for Help context ID.
        Public Const HELP_QUIT = &H2            'Terminate Help.
        Public Const HELP_INDEX = &H3           'Display Help index.
        Public Const HELP_CONTEXTPOPUP = &H8&   'Display Help context as a pop-up
        'window.
        Public Const HELP_FINDER = &HB&         'If found, Display container file.
        Public Const HELP_KEY = &H101           'Display topic for keyword.

        'Declare the WinHelp function.
        Declare Sub WinHelp Lib "user32" Alias "WinHelpA" (ByVal Hwnd As Long, ByVal lpHelpFile As String, ByVal wCommand As Long, ByVal dwData As Long)

        '        Function OpenHelpContainer(ByVal strHelpFileName As String)
        'Opens the Help container.
        '    WinHelp (Application.hWndAccessApp, byval strHelpFileName, HELP_FINDER, ByVal vbNullString)
        '        End Function

        '        Function OpenHelpIndex(ByVal strHelpFileName As String)
        'Opens the Help index.
        '    WinHelp Application.hWndAccessApp, ByVal strHelpFileName, HELP_KEY, _
        'ByVal ""
        'End Function

        'Function OpenHelpIndexWithSearchKey(ByVal strHelpFileName As String, _
        '        ByVal strSearchKey As String)
        'Opens the Help index and searches for keyword SKey.
        'WinHelp Application.hWndAccessApp, ByVal strHelpFileName, HELP_KEY, _
        'ByVal strSearchKey
        'End Function

        'Function OpenHelpWithContextID(ByVal strHelpFileName As String, _
        'ByVal lngContextID As Long)
        'Opens the Help file to ContextID.
        'WinHelp Application.hWndAccessApp, ByVal strHelpFileName, _
        'HELP_CONTEXT, ByVal lngContextID
        'End Function

        'Function OpenHelpWithContextIDPopup(ByVal strHelpFileName As String, _
        'ByVal lngContextID As Long)
        'Opens the Help file to ContextID as a pop-up window.
        'WinHelp Application.hWndAccessApp, ByVal strHelpFileName, _
        'HELP_CONTEXTPOPUP, ByVal lngContextID
        'End Function

        'Function CloseHelpContainer(ByVal strHelpFileName As String)
        'Closes the specified Help file.
        'WinHelp Application.hWndAccessApp, ByVal strHelpFileName, HELP_QUIT, _
        'ByVal vbNullString
        'End Function
    End Class
End Namespace


'1.	OpenHelpContainer()
'You can use the OpenHelpContainer function simply to open a Help file. To test this function, type the following in the Immediate window, and then press ENTER:

'?OpenHelpContainer("Calc.hlp")


'This opens the Help file for the Microsoft Windows Calculator.
'2.	OpenHelpIndex()
'Opens the Help file with the Index tab activated. To test this function, type the following in the Immediate window, and then press ENTER:

'?OpenHelpIndex("Calc.hlp")


'This opens the Help file for the Microsoft Windows Calculator with the Index tab displayed.
'3.	OpenHelpIndexWithSearchKey()
'Opens the specified Help index and searches for a keyword. To test this function, type the following in the Immediate window, and then press ENTER:

'?OpenHelpIndexWithSearchKey("calc.hlp","simple calculations")


'Note that the "Simple Calculations" topic ID is displayed.
'4.	OpenHelpWithContextID()
'Opens the Help file to a specified ContextID. To test this function, type the following in the Immediate window, and then press ENTER:

'?OpenHelpWithContextID("calc.hlp",90)


'Note that it opens Help for the division button.
'5.	OpenHelpWithContextIDPopup()
'Opens the Help file to a specified ContextID as a pop-up window. To test this function, type the following in the Immediate window, and then press ENTER:

'?OpenHelpWithContextIDPopup("Calc.hlp",90)


'Note that it opens Help for the division button in a pop-up window.
'6.	CloseHelpContainer()
'Closes the Help file. To test this function, type the following in the Immediate window, and then press ENTER:

'?CloseHelpContainer("Calc.hlp")


'This should close the Microsoft Windows Calculator Help file if it is open. 