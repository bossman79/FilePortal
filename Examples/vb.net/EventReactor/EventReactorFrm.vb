Public Class EventReactorFrm
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Message As System.Windows.Forms.Label
    Friend WithEvents Message2 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.Button1 = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Message = New System.Windows.Forms.Label
        Me.Message2 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(24, 16)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(240, 32)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Button1"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(16, 72)
        Me.Label1.Name = "Label1"
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Label1"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(16, 136)
        Me.Label2.Name = "Label2"
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Label2"
        '
        'Message
        '
        Me.Message.Location = New System.Drawing.Point(128, 72)
        Me.Message.Name = "Message"
        Me.Message.Size = New System.Drawing.Size(144, 56)
        Me.Message.TabIndex = 3
        Me.Message.Text = "Waiting for a command to begin in Adept..."
        '
        'Message2
        '
        Me.Message2.Location = New System.Drawing.Point(128, 136)
        Me.Message2.Name = "Message2"
        Me.Message2.Size = New System.Drawing.Size(144, 56)
        Me.Message2.TabIndex = 4
        Me.Message2.Text = "Waiting for a command to begin in Adept..."
        '
        'EventReactorFrm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(292, 214)
        Me.Controls.Add(Me.Message2)
        Me.Controls.Add(Me.Message)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Button1)
        Me.Name = "EventReactorFrm"
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "m_"
    Dim m_Registered As Boolean
    Dim m_EventReactorClass As PI_EventReactor
    Dim m_PlugIn As Interop.AdeptCAC.NxPlugIn
#End Region

#Region "Form"

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_Registered = False
        Button1.Text = "Register Event Reactor"
        Message.Text = "Press the Register Event Reactor button to begin."
        Message2.Text = ""
    End Sub

    Private Sub EventReactorFrm_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        End
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim rtn As Long
        If (m_Registered = False) Then
            ' register
            m_EventReactorClass = New PI_EventReactor
            m_PlugIn = g_Project.PlugInManager.PlugInList.RegisterRuntimePlugIn(m_EventReactorClass, "Event Reactor", "Runtime Registration Event Reactor Example Program")

            Button1.Text = "Unregister Event Reactor"
            m_Registered = True
        Else
            ' unregister
            m_PlugIn = Nothing

            Dim PIM As Interop.AdeptCAC.NxPlugInManager
            Dim PIL As Interop.AdeptCAC.NxPlugInList
            PIM = g_Project.PlugInManager
            PIL = PIM.PlugInList

            ' not sure why this throwing an error. everything looks fine
            'rtn = g_Project.PlugInManager.PlugInList.UnregisterRuntimePlugIn(m_EventReactorClass)
            'rtn =
            On Error Resume Next
            Call PIL.UnregisterRuntimePlugIn(m_EventReactorClass)
            On Error GoTo 0

            PIL = Nothing
            PIM = Nothing
            m_EventReactorClass = Nothing

            Button1.Text = "Register Event Reactor"
            m_Registered = False
        End If
    End Sub


    Private Delegate Sub SetMessageDelegate(ByVal bMsg1 As Boolean, ByVal msg As String)
    Public Sub SetMessage(ByVal bMsg1 As Boolean, ByVal msg As String)
        If Me.InvokeRequired Then
            Me.Invoke(New SetMessageDelegate(AddressOf SetMessage), bMsg1, msg)
        Else
            If (bMsg1) Then
                Me.Message.Text = msg
            Else
                Me.Message2.Text = msg
            End If
        End If
    End Sub


#End Region

End Class
