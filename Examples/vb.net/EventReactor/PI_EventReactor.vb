
Imports Synergis.Adept.MainApi

Public Class PI_EventReactor
    Implements Interop.AdeptCAC.INxPlugInInterface
    Implements Interop.AdeptCAC.INxPlugInGuiEvents

    Public Function Initialize(ByVal PlugInId As String, ByVal Project As Interop.AdeptCAC.NxProject, ByVal GuiApiObj As Object) As Integer Implements Interop.AdeptCAC.INxPlugInInterface.Initialize
        MsgBox("PI_EventReactor - INxPlugInInterface_Initialize")
        Initialize = 1
    End Function

    Public Sub Uninitialize() Implements Interop.AdeptCAC.INxPlugInInterface.Uninitialize
        MsgBox("PI_EventReactor - INxPlugInInterface_Uninitialize")
    End Sub

    Public Function AllowLibCardEnable(ByVal LibCardMode As Integer, ByVal DocRec As Interop.AdeptCAC.NxDocRecord, ByVal FieldName As String, ByVal bForUpdateDocument As Integer, ByRef pbExtractSupported As Integer, ByRef pbUpdateDocumentSupported As Integer) As Integer Implements Interop.AdeptCAC.INxPlugInGuiEvents.AllowLibCardEnable

    End Function

    Public Function BeginACommand(ByVal CommandNumber As Integer, ByVal CommandName As String, ByVal WindowTbl As Interop.AdeptCAC.NxTbl, ByVal DetailedList As Interop.AdeptCAC.NxDetailedList) As Integer Implements Interop.AdeptCAC.INxPlugInGuiEvents.BeginACommand
        g_ERFrm.SetMessage(True, CommandName) 'g_ERFrm.Message.Text = CommandName
        g_ERFrm.SetMessage(False, "") 'g_ERFrm.Message2.Text = ""
        BeginACommand = ApiTypes.eContinue
    End Function

    Public Sub BeginCustomCommand(ByVal CommandNumber As Integer, ByVal WindowTbl As Interop.AdeptCAC.NxTbl, ByVal DetailedList As Interop.AdeptCAC.NxDetailedList) Implements Interop.AdeptCAC.INxPlugInGuiEvents.BeginCustomCommand

    End Sub

    Public Sub EndACommand(ByVal CommandNumber As Integer, ByVal CommandName As String, ByVal WindowTbl As Interop.AdeptCAC.NxTbl, ByVal DetailedList As Interop.AdeptCAC.NxDetailedList) Implements Interop.AdeptCAC.INxPlugInGuiEvents.EndACommand
        g_ERFrm.SetMessage(True, "") ' g_ERFrm.Message.Text = ""
        g_ERFrm.SetMessage(False, CommandName) 'g_ERFrm.Message2.Text = CommandName
    End Sub

    Public Sub EndCustomCommand(ByVal CommandNumber As Integer, ByVal WindowTbl As Interop.AdeptCAC.NxTbl, ByVal DetailedList As Interop.AdeptCAC.NxDetailedList) Implements Interop.AdeptCAC.INxPlugInGuiEvents.EndCustomCommand

    End Sub

    Public Function LibraryCardFieldGetFocus(ByVal FieldName As String) As Integer Implements Interop.AdeptCAC.INxPlugInGuiEvents.LibraryCardFieldGetFocus

    End Function

    Public Function LibraryCardFieldLoseFocus(ByVal FieldName As String) As Integer Implements Interop.AdeptCAC.INxPlugInGuiEvents.LibraryCardFieldLoseFocus

    End Function
End Class
