Module EventReactorMod

    Public g_Project As Interop.AdeptCAC.NxProject
    Public g_GuiApi As Interop.AdeptGui.GuiApi
    Public g_ERFrm As EventReactorFrm

    Sub Main()
        On Error GoTo Handler
        g_GuiApi = CreateObject("AdeptGui.GuiApi")
        If (g_GuiApi Is Nothing) Then
            GoTo Handler
        End If
        g_Project = g_GuiApi.GetProject()
        If (g_Project Is Nothing) Then
            GoTo Handler
        End If
        On Error GoTo 0
        g_ERFrm = New EventReactorFrm
        g_ERFrm.ShowDialog()
        Exit Sub
Handler:
        MsgBox("Failed to connect to Adept.")
    End Sub
End Module
