Imports System.IO
Imports System.Net.Security
Imports System.Net.WebRequestMethods

Public Class Form1

    Dim StartRunTime = Date.Now
    Dim OyajiMode As Boolean = False 'if True it skips selecting location for building files. EXE will need to be located in root working DIR

    Private Sub btnCreateANIM_Click(sender As Object, e As EventArgs) Handles btnCreateANIM.Click
        Dim WriteANIMHeader As Boolean
        Dim InputDIR As String
        Dim orgAnimeDIR As String


        'Check if we are running OyajiMode to bypass file location inputs. Just hardcoded for ez
        If OyajiMode = True Then
            InputDIR = "anims\jyanpai"
            orgAnimeDIR = "cd\working\jyanpai"
        Else
            'Get the Directoy where the completed cel files are
            If InputDIR = Nothing Then
                'Get ORG CEL Input DIR
                fbdInputFolder.Description = "Select the root location of anim CELS (anims/jyanpai) to create a ANIM from"
                fbdInputFolder.InitialDirectory = Application.ExecutablePath
                If fbdInputFolder.ShowDialog() = DialogResult.OK Then
                    InputDIR = fbdInputFolder.SelectedPath
                Else
                    Exit Sub
                End If
            End If

            'Get Directory of where the ORG ANIM Files are
            If orgAnimeDIR = Nothing Then
                'Get ORG CEL Input DIR
                fbsOrgANIMDir.Description = "Select the root location (working/jyanpai) of where the ORG ANIM are"
                fbsOrgANIMDir.InitialDirectory = Application.ExecutablePath
                If fbsOrgANIMDir.ShowDialog() = DialogResult.OK Then
                    orgAnimeDIR = fbsOrgANIMDir.SelectedPath
                Else
                    Exit Sub
                End If
            End If
        End If


        'Check if we should write the ANIM HEX Header
        If chkbxAnimHeader.Checked = True Then
            WriteANIMHeader = True
        Else
            WriteANIMHeader = False
        End If

        'Lets get the cels for the ANIM FILE
        Dim CompletedDIR = ""
        For Each f In Directory.GetFiles(InputDIR, "*.cel", SearchOption.AllDirectories)
            Dim CurrentFile = Path.GetFileName(f)
            Dim CurrentDirectory = Path.GetDirectoryName(f)
            Dim ORGANIMFileLocation = orgAnimeDIR & CurrentDirectory.Split("jyanpai")(1)
            Dim ORGANIMFile = ORGANIMFileLocation & ".ANIM"

            'This will isolate our BW to this folder only.
            If CurrentDirectory <> CompletedDIR Then
                For Each c In Directory.GetFiles(CurrentDirectory, "*.cel")
                    CompletedDIR = CurrentDirectory
                    Dim CELtoUSE = c
                    Dim OutputANIMFile = ORGANIMFile

                    'Write the first cel file into a ANIM File
                    If Path.GetFileNameWithoutExtension(c).ToUpper.EndsWith("0") And Not Path.GetFileName(c).ToUpper = "MAINMENU.ANIM10.CEL" Or Path.GetFileName(c).ToUpper.StartsWith("MENU_FONT[0]") Then
                        'First check if we need write the anim header
                        If WriteANIMHeader = True Then
                            Dim ANIMHeaderBytes
                            'Get First 0x30 bytes
                            Using br As BinaryReader = New BinaryReader(IO.File.Open(ORGANIMFile, FileMode.Open))
                                ANIMHeaderBytes = br.ReadBytes(&H30)
                            End Using

                            'If File exist delete
                            If IO.File.Exists(OutputANIMFile) Then
                                IO.File.Delete(OutputANIMFile)
                            End If

                            'Write Header
                            Using bw As BinaryWriter = New BinaryWriter(IO.File.Open(OutputANIMFile, FileMode.Create))
                                bw.Write(ANIMHeaderBytes)
                            End Using
                            'Write First File
                            Dim File0Bytes = IO.File.ReadAllBytes(c)
                            Using bw As BinaryWriter = New BinaryWriter(IO.File.Open(OutputANIMFile, FileMode.Append))
                                bw.Write(File0Bytes)
                            End Using
                        Else
                            'If File exist delete
                            If IO.File.Exists(OutputANIMFile) Then
                                IO.File.Delete(OutputANIMFile)
                            End If

                            Dim File0Bytes = IO.File.ReadAllBytes(c)
                            Using bw As BinaryWriter = New BinaryWriter(IO.File.Open(OutputANIMFile, FileMode.OpenOrCreate))
                                bw.Write(File0Bytes)
                            End Using
                        End If


                    Else
                        'Get PDAT Bytes from File
                        Dim PDATBytes
                        Using br As BinaryReader = New BinaryReader(IO.File.Open(c, FileMode.Open))
                            br.BaseStream.Seek(&H50, SeekOrigin.Begin) 'seek to start of pdat (hopefully)
                            Dim CheckBytes = br.ReadBytes(2)
                            If System.Text.Encoding.UTF8.GetString(CheckBytes) = "PL" Then ' If the check bytes = PL then we need to move to the next sector for PDAT
                                If c.ToUpper.Contains("MAINMENU.ANIM") Or c.ToUpper.Contains("OYAJI.ANIM") Or c.ToUpper.Contains("OSIOKI.ANIM") Or c.ToUpper.Contains("CHANCE.ANIM") Or c.ToUpper.Contains("FUKIDASI0") Or c.ToUpper.Contains("MENU_FONT") Or c.ToUpper.Contains("LAST.ANIM") Then
                                    br.BaseStream.Seek(&H50, SeekOrigin.Begin)
                                    PDATBytes = br.ReadBytes(br.BaseStream.Length - br.BaseStream.Position)
                                Else
                                    br.BaseStream.Seek(&H9C, SeekOrigin.Begin)
                                    PDATBytes = br.ReadBytes(br.BaseStream.Length - br.BaseStream.Position)
                                End If
                            Else
                                br.BaseStream.Seek(&H50, SeekOrigin.Begin)
                                PDATBytes = br.ReadBytes(br.BaseStream.Length - br.BaseStream.Position)
                            End If
                        End Using

                        'Write PDAT to new ANIM File
                        Using bw As BinaryWriter = New BinaryWriter(IO.File.Open(OutputANIMFile, FileMode.Append))
                            bw.Write(PDATBytes)
                        End Using
                    End If
                Next
            End If
        Next

        MessageBox.Show("Built ANIM files and put into working dir.")

    End Sub

    Private Sub btnCreateAnimCEls_Click(sender As Object, e As EventArgs) Handles btnCreateAnimCEls.Click
        Dim InputDIR As String

        'Get the Directoy where the completed BMPs files are
        'If Oyajimode enabled we skip and just hardcode
        If OyajiMode = True Then
            InputDIR = "anims\jyanpai"
        Else
            If InputDIR = Nothing Then
                'Get ORG BMP Input DIR
                fbdInputFolder.Description = "Select the root location of BMP to create CEL's from"
                fbdInputFolder.InitialDirectory = Application.ExecutablePath
                If fbdInputFolder.ShowDialog() = DialogResult.OK Then
                    InputDIR = fbdInputFolder.SelectedPath
                Else
                    Exit Sub
                End If
            End If
        End If


        'Find the BMP and convert into CELS
        For Each f In Directory.GetFiles(InputDIR, "*.bmp", SearchOption.AllDirectories)
            Dim BMPFilename = Path.GetFileName(f).ToUpper
            Dim BMPToUSE = f
            Dim OutputCEL = Path.GetDirectoryName(f) & "\" & Path.GetFileNameWithoutExtension(f)

            Dim FileInfoArgument As String
            'NOT PACKED BUT CODED 4BPP
            If BMPFilename.StartsWith("MAINMENU.ANIM") Then
                FileInfoArgument = "-b 4 --packed false --coded true --lrform false --ccb-skip unset --ccb-last set --ccb-npabs set --ccb-spabs set --ccb-ppabs set --ccb-ldsize set --ccb-ldprs set --ccb-ldplut set --ccb-ccbpre set --ccb-yoxy set --ccb-acsc unset --ccb-alsc unset --ccb-acw set --ccb-accw set --ccb-twd set --ccb-lce unset --ccb-ace unset --ccb-maria unset --ccb-pxor unset --ccb-useav unset --ccb-plutpos unset --ccb-bgnd unset --ccb-noblk unset --pre0-literal unset --pre0-bgnd unset --pre0-rep8 unset"

                'PACKED & CODED 4BPP BGND=FALSE
            ElseIf BMPFilename.StartsWith("LAST.ANIM") Or BMPFilename.StartsWith("SUB_TITLE0") Then
                FileInfoArgument = "-b 4 --packed true --coded true --ccb-ldplut set --ccb-ace unset --ccb-bgnd unset --ccb-noblk unset"

                'PACKED & CODED 4BPP BGND=TRUE
            ElseIf BMPFilename.StartsWith("SUB_TITLE0") Then
                FileInfoArgument = "-b 4 --packed true --coded true --ccb-ldplut set --ccb-ace unset --ccb-bgnd set --ccb-noblk unset"

                'PACKED & CODED 6BPP BGND=FALSE
            ElseIf BMPFilename.StartsWith("KYUSYU.ANIM") Or BMPFilename.StartsWith("OSIOKI.ANIM") Or BMPFilename.StartsWith("CHANCE.ANIM") Or BMPFilename.StartsWith("MENU_FONT[") Then
                FileInfoArgument = "-b 6 --packed true --coded true --ccb-ldplut set --ccb-ace unset --ccb-bgnd unset --ccb-noblk unset"

                'PACKED & CODED 6BPP BGND=TRUE 
            ElseIf BMPFilename.StartsWith("OYAJI.ANIM") Or BMPFilename.StartsWith("FUKIDASI") Then
                FileInfoArgument = "-b 6 --packed true --coded true --ccb-ldplut set --ccb-ace unset --ccb-bgnd set --ccb-noblk unset"

                'PACKED & CODED 16BPP
            ElseIf BMPFilename.StartsWith("ATTACK.ANIM") Then
                FileInfoArgument = "-b 16 --packed true --coded false --ccb-ldplut unset --ccb-ace unset --ccb-bgnd unset --ccb-noblk unset"

                'Everything else is here
            Else
                FileInfoArgument = "-b 8 --packed true --coded false --ccb-ldplut set --ccb-ace unset --ccb-bgnd unset --ccb-noblk unset"
            End If
            Dim To_CEL_Argu = "/c .\3it_win64.exe to-cel -o " & OutputCEL & " " & FileInfoArgument & " " & BMPToUSE
            Dim oShell = Shell("cmd.exe " & To_CEL_Argu, AppWinStyle.Hide, True, 10000)

            ListBox1.Items.Add("Created Anim CEL from " & Path.GetFileName(f))

        Next

        MessageBox.Show("Completed building CEL's for ANIM")
    End Sub

    'Create CELS
    Private Sub btnGetInfo_Click(sender As Object, e As EventArgs) Handles btnGetInfo.Click
        GetCELInfo()
    End Sub

    Private Sub btnCleanupInfo_Click(sender As Object, e As EventArgs) Handles btnCleanupInfo.Click
        CleaupCELINFO()
    End Sub

    Private Sub btnCreateImages_Click(sender As Object, e As EventArgs) Handles btnCreateImages.Click
        Dim RunPath As String = Application.ExecutablePath
        Dim RunDIR As String = Path.GetDirectoryName(RunPath)
        Dim WorkingDIR As String = "cd\working\"
        Dim NewImagesDIR As String = "images\"
        Dim ImageTool As String = "3it_win64.exe"
        Dim InputDIR As String
        Dim BMPDIR As String
        If IO.File.Exists(ImageTool) = False Then
            MessageBox.Show("Missing 3IT tool")
            Exit Sub
        End If

        'If oyajimode set we skip and hardcode for ez
        If OyajiMode = True Then
            InputDIR = "cd\working\jyanpai"
            BMPDIR = "images\jyanpai"
        Else
            'Get ORG CEL Input DIR
            If InputDIR = Nothing Then
                fbdInputFolder.Description = "Select the ORG CEL images root DIR"
                fbdInputFolder.InitialDirectory = Application.ExecutablePath
                If fbdInputFolder.ShowDialog() = DialogResult.OK Then
                    InputDIR = fbdInputFolder.SelectedPath
                Else
                    Exit Sub
                End If
            End If

            'Set the Location of the new BMP to create CEL's from
            fbdNewBMPDIR.Description = "Select the BMP images root DIR, to create CELS from"
            fbdNewBMPDIR.InitialDirectory = Application.ExecutablePath
            If fbdNewBMPDIR.ShowDialog() = DialogResult.OK Then
                BMPDIR = fbdNewBMPDIR.SelectedPath
            Else
                Exit Sub
            End If

        End If


        'Start Gathering info about CEL  - Info file will be stored alongside the OG CEL location 
        For Each f In Directory.GetFiles(InputDIR, "*.cel", SearchOption.AllDirectories)
            Dim CELFilename = Path.GetFileName(f)
            Dim CELDirectory = Path.GetDirectoryName(f)
            Dim InfoOutput = Path.GetDirectoryName(f) & "\" & Path.GetFileName(f) & "_info.txt"

            'Parse and get the Arguement needed to create the CEL
            Dim FileInfoArgument = ParseCELINFO(InfoOutput)

            'Next Start Creating the new CEL's These will be put into a working folder where the OGs are stored.
            Dim NewBMPImageName = CELFilename & ".bmp"
            Dim NewBMPDir
            If CELDirectory.Contains("jyanpai") Then
                NewBMPDir = BMPDIR & CELDirectory.Split("jyanpai")(1)
            ElseIf CELDirectory.Contains("MajalisArt") Then
                NewBMPDir = BMPDIR & CELDirectory.Split("MajalisArt")(1)
            End If

            Dim BMPToUSE = NewBMPDir & "\" & NewBMPImageName

            If IO.File.Exists(BMPToUSE) = False Then
                ListBox1.Items.Add("Couldnt find " & NewBMPImageName)
                'MessageBox.Show("Missing " & NewBMPImageName & vbNewLine & f & vbNewLine & BMPToUSE)
            Else
                Dim To_CEL_Argu = "/c .\3it_win64.exe to-cel -o " & f & " --external-palette=" & f & " " & FileInfoArgument & " " & BMPToUSE

                Dim oShell = Shell("cmd.exe " & To_CEL_Argu, AppWinStyle.Hide, True, 10000)

                ListBox1.Items.Add("Created CEL from " & NewBMPImageName)
            End If

        Next

        MessageBox.Show("Completed Creating CEL's")
    End Sub


    'FUNCTIONS
    Public Function CheckIfFileCreated(ByVal inputfile As String)
        If IO.File.GetLastWriteTime(inputfile) > StartRunTime Then

        End If
    End Function

    Public Function CleaupCELINFO()
        Dim InputDIR As String

        If OyajiMode = True Then
            InputDIR = "cd\working\jyanpai"
        Else
            If InputDIR = Nothing Then
                'Get ORG CEL Input DIR
                fbdInputFolder.Description = "Select the ORG CEL images root DIR"
                fbdInputFolder.InitialDirectory = Application.ExecutablePath
                If fbdInputFolder.ShowDialog() = DialogResult.OK Then
                    InputDIR = fbdInputFolder.SelectedPath
                Else
                    Exit Function
                End If
            End If
        End If


        'Generate the CEL INFO Text Files
        For Each f In Directory.GetFiles(InputDIR, "*.cel", SearchOption.AllDirectories)
            Dim InfoOutput = Path.GetDirectoryName(f) & "\" & Path.GetFileName(f) & "_info.txt"
            If IO.File.Exists(InfoOutput) = True Then
                IO.File.Delete(InfoOutput)
                ListBox1.Items.Add("Deleted Info file at " & InfoOutput)
            End If
        Next

        MessageBox.Show("Cleaned up CEL INFO")
    End Function

    Public Function GetCELInfo()
        Dim InputDIR As String

        If OyajiMode = True Then
            InputDIR = "cd\working\jyanpai"
        Else
            If InputDIR = Nothing Then
                'Get ORG CEL Input DIR
                fbdInputFolder.Description = "Select the ORG CEL images root DIR"
                fbdInputFolder.InitialDirectory = Application.ExecutablePath
                If fbdInputFolder.ShowDialog() = DialogResult.OK Then
                    InputDIR = fbdInputFolder.SelectedPath
                Else
                    Exit Function
                End If
            End If
        End If

        'Generate the CEL INFO Text Files
        For Each f In Directory.GetFiles(InputDIR, "*.cel", SearchOption.AllDirectories)
            Dim InfoOutput = Path.GetDirectoryName(f) & "\" & Path.GetFileName(f) & "_info.txt"
            Dim ARGU = "/c .\3it_win64.exe info " & f & " > " & InfoOutput
            Dim oShell = Shell("cmd.exe " & ARGU, AppWinStyle.Hide, True, 10000)

            ListBox1.Items.Add("Created Info file at " & InfoOutput)
        Next
        MessageBox.Show("Generated CEL INFO")
    End Function

    Public Function ParseCELINFO(ByVal inputfile As String)
        'Flags Var
        Dim BPP As Integer
        Dim EXTERNALPALETTE As String
        Dim TRANSPARENT As String = "magenta"

        'These flags use true/false as its boolean
        Dim CODED As Boolean
        Dim LRFORM As Boolean
        Dim PACKED As Boolean

        'These flags use set/unset as its boolean
        Dim CCB_SKIP As String
        Dim CCB_LAST As String
        Dim CCB_NPABS As String
        Dim CCB_SPABS As String
        Dim CCB_PPABS As String
        Dim CCB_LDSIZE As String
        Dim CCB_LDPRS As String
        Dim CCB_LDPLUT As String
        Dim CCB_CCBPRE As String
        Dim CCB_YOXY As String
        Dim CCB_ACSC As String
        Dim CCB_ALSC As String
        Dim CCB_ACW As String
        Dim CCB_ACCW As String
        Dim CCB_TWD As String
        Dim CCB_LCE As String
        Dim CCB_ACE As String
        Dim CCB_MARIA As String
        Dim CCB_PXOR As String
        Dim CCB_USEAV As String
        Dim CCB_PACKED As String
        Dim CCB_PLUTPOS As String
        Dim CCB_BGND As String
        Dim CCB_NOBLK As String
        Dim PRE0_LITERAL As String
        Dim PRE0_BGND As String
        Dim PRE0_UNCODED As String
        Dim PRE0_REP8 As String
        Dim PRE1_LRFORM As String

        Dim LineCount = 0
        Dim Infolines = IO.File.ReadAllLines(inputfile)
        For Each L In Infolines ''Transperency is not included here
            If L.Contains("- bpp:") Then
                Dim BPPSplit = L.Split("(")(1)
                BPPSplit = BPPSplit.Substring(0, 2).Trim()
                If BPPSplit.Contains("b") Then
                    BPPSplit = BPPSplit.Replace("b", "")
                End If
                BPP = BPPSplit

                ''CCB Flags
            ElseIf L.Contains("- skip:") Then
                If L.Contains("true") Then
                    CCB_SKIP = "set"
                Else
                    CCB_SKIP = "unset"
                End If
            ElseIf L.Contains("- last:") Then
                If L.Contains("true") Then
                    CCB_LAST = "set"
                Else
                    CCB_LAST = "unset"
                End If
            ElseIf L.Contains("- npabs:") Then
                If L.Contains("true") Then
                    CCB_NPABS = "set"
                Else
                    CCB_NPABS = "unset"
                End If
            ElseIf L.Contains("- spabs:") Then
                If L.Contains("true") Then
                    CCB_SPABS = "set"
                Else
                    CCB_SPABS = "unset"
                End If
            ElseIf L.Contains("- ppabs:") Then
                If L.Contains("true") Then
                    CCB_PPABS = "set"
                Else
                    CCB_PPABS = "unset"
                End If
            ElseIf L.Contains("- ldsize:") Then
                If L.Contains("true") Then
                    CCB_LDSIZE = "set"
                Else
                    CCB_LDSIZE = "unset"
                End If
            ElseIf L.Contains("- ldprs:") Then
                If L.Contains("true") Then
                    CCB_LDPRS = "set"
                Else
                    CCB_LDPRS = "unset"
                End If
            ElseIf L.Contains("- ldplut:") Then
                If L.Contains("true") Then
                    CCB_LDPLUT = "set"
                Else
                    CCB_LDPLUT = "unset"
                End If
            ElseIf L.Contains("- ccbpre:") Then
                If L.Contains("true") Then
                    CCB_CCBPRE = "set"
                Else
                    CCB_CCBPRE = "unset"
                End If
            ElseIf L.Contains("- yoxy:") Then
                If L.Contains("true") Then
                    CCB_YOXY = "set"
                Else
                    CCB_YOXY = "unset"
                End If
            ElseIf L.Contains("- acsc:") Then
                If L.Contains("true") Then
                    CCB_ACSC = "set"
                Else
                    CCB_ACSC = "unset"
                End If
            ElseIf L.Contains("- alsc:") Then
                If L.Contains("true") Then
                    CCB_ALSC = "set"
                Else
                    CCB_ALSC = "unset"
                End If
            ElseIf L.Contains("- acw:") Then
                If L.Contains("true") Then
                    CCB_ACW = "set"
                Else
                    CCB_ACW = "unset"
                End If
            ElseIf L.Contains("- accw:") Then
                If L.Contains("true") Then
                    CCB_ACCW = "set"
                Else
                    CCB_ACCW = "unset"
                End If
            ElseIf L.Contains("- twd:") Then
                If L.Contains("true") Then
                    CCB_TWD = "set"
                Else
                    CCB_TWD = "unset"
                End If
            ElseIf L.Contains("- lce:") Then
                If L.Contains("true") Then
                    CCB_LCE = "set"
                Else
                    CCB_LCE = "unset"
                End If
            ElseIf L.Contains("- ace:") Then
                If L.Contains("true") Then
                    CCB_ACE = "set"
                Else
                    CCB_ACE = "unset"
                End If
            ElseIf L.Contains("- maria:") Then
                If L.Contains("true") Then
                    CCB_MARIA = "set"
                Else
                    CCB_MARIA = "unset"
                End If
            ElseIf L.Contains("- pxor:") Then
                If L.Contains("true") Then
                    CCB_PXOR = "set"
                Else
                    CCB_PXOR = "unset"
                End If
            ElseIf L.Contains("- useav:") Then
                If L.Contains("true") Then
                    CCB_USEAV = "set"
                Else
                    CCB_USEAV = "unset"
                End If
            ElseIf L.Contains("- packed:") Then 'If true has additonal flags set
                If L.Contains("true") Then
                    CCB_PACKED = "set"
                    PACKED = True
                Else
                    CCB_PACKED = "unset"
                    PACKED = False
                End If
            ElseIf L.Contains("- plutpos:") Then
                If L.Contains("true") Then
                    CCB_PLUTPOS = "set"
                Else
                    CCB_PLUTPOS = "unset"
                End If
            ElseIf L.Contains("- bgnd:") And LineCount = 26 Then
                If L.Contains("true") Then
                    CCB_BGND = "set"
                Else
                    CCB_BGND = "unset"
                End If
            ElseIf L.Contains("- noblk:") Then
                If L.Contains("true") Then
                    CCB_NOBLK = "set"
                Else
                    CCB_NOBLK = "unset"
                End If

                ''Pre0 Flags
            ElseIf L.Contains("- literal:") Then
                If L.Contains("true") Then
                    PRE0_LITERAL = "set"
                Else
                    PRE0_LITERAL = "unset"
                End If
            ElseIf L.Contains("- bgnd:") And LineCount = 34 Then
                If L.Contains("true") Then
                    PRE0_BGND = "set"
                Else
                    PRE0_BGND = "unset"
                End If
            ElseIf L.Contains("- uncoded:") Then 'If true has additonal flags set
                If L.Contains("true") Then
                    PRE0_UNCODED = "set"
                    CODED = False
                Else
                    PRE0_UNCODED = "unset"
                    CODED = True
                End If
            ElseIf L.Contains("- rep8:") Then
                If L.Contains("true") Then
                    PRE0_REP8 = "set"
                Else
                    PRE0_REP8 = "unset"
                End If

                ''Pre1 Flags
            ElseIf L.Contains("- lrform:") Then 'If true has additonal flags set
                If L.Contains("true") Then
                    PRE1_LRFORM = "set"
                    LRFORM = True
                Else
                    PRE1_LRFORM = "unset"
                    LRFORM = False
                End If
            End If
            LineCount += 1
        Next

        'Time to craft the argu for 3IT
        Dim CELDirectory = Path.GetDirectoryName(inputfile).ToUpper
        Dim Filename = Path.GetFileName(inputfile).ToUpper
        Dim Argument As String

        'For files that dont need transparent
        If Filename.StartsWith("WAIT0") Then
            Argument = "-b " & BPP & " --coded " & CODED & " --lrform " & LRFORM & " --packed " & PACKED & " --ccb-skip " & CCB_SKIP & " --ccb-last " & CCB_LAST & " --ccb-npabs " & CCB_NPABS & " --ccb-spabs " & CCB_SPABS & " --ccb-ppabs " & CCB_PPABS & " --ccb-ldsize " & CCB_LDSIZE & " --ccb-ldprs " & CCB_LDPRS & " --ccb-ldplut " & CCB_LDPLUT & " --ccb-ccbpre " & CCB_CCBPRE & " --ccb-yoxy " & CCB_YOXY & " --ccb-acsc " & CCB_ACSC & " --ccb-alsc " & CCB_ALSC & " --ccb-acw " & CCB_ACW & " --ccb-accw " & CCB_ACCW & " --ccb-twd " & CCB_TWD & " --ccb-lce " & CCB_LCE & " --ccb-ace " & CCB_ACE & " --ccb-maria " & CCB_MARIA & " --ccb-pxor " & CCB_PXOR & " --ccb-useav " & CCB_USEAV & " --ccb-plutpos " & CCB_PLUTPOS & " --ccb-bgnd " & CCB_BGND & " --ccb-noblk " & CCB_NOBLK & " --pre0-literal " & PRE0_LITERAL & " --pre0-bgnd " & PRE0_BGND & " --pre0-rep8 " & PRE0_REP8

            'This one is for Majin Minigame Title Image
        ElseIf Filename.StartsWith("TITLE_WIN") And CELDirectory.Contains("MAJALISART") Then
            Argument = "" 'Dont ask me why this works?

        ElseIf Filename.StartsWith("FUKIDASI01") And CELDirectory.Contains("YOBIKOU") Then
            Argument = "-b 16 --coded False --packed True --transparent magenta --ccb-bgnd unset --ccb-noblk set"

            'For Tutorial Prompts
        ElseIf Filename.StartsWith("TITLE_") Then
            Argument = "-b " & BPP & " --coded " & CODED & " --lrform " & LRFORM & " --packed " & PACKED & " --ccb-skip " & CCB_SKIP & " --ccb-last " & CCB_LAST & " --ccb-npabs " & CCB_NPABS & " --ccb-spabs " & CCB_SPABS & " --ccb-ppabs " & CCB_PPABS & " --ccb-ldsize " & CCB_LDSIZE & " --ccb-ldprs " & CCB_LDPRS & " --ccb-ldplut " & CCB_LDPLUT & " --ccb-ccbpre " & CCB_CCBPRE & " --ccb-yoxy " & CCB_YOXY & " --ccb-acsc " & CCB_ACSC & " --ccb-alsc " & CCB_ALSC & " --ccb-acw " & CCB_ACW & " --ccb-accw " & CCB_ACCW & " --ccb-twd " & CCB_TWD & " --ccb-lce " & CCB_LCE & " --ccb-ace " & CCB_ACE & " --ccb-maria " & CCB_MARIA & " --ccb-pxor " & CCB_PXOR & " --ccb-useav " & CCB_USEAV & " --ccb-plutpos " & CCB_PLUTPOS & " --ccb-bgnd " & "set" & " --ccb-noblk " & CCB_NOBLK & " --pre0-literal " & PRE0_LITERAL & " --pre0-bgnd " & PRE0_BGND & " --pre0-rep8 " & PRE0_REP8

        ElseIf Filename.StartsWith("TAN0") Then
            Argument = "-b " & "8" & " --coded " & False & " --lrform " & LRFORM & " --packed " & PACKED & " --ccb-skip " & CCB_SKIP & " --ccb-last " & CCB_LAST & " --ccb-npabs " & CCB_NPABS & " --ccb-spabs " & CCB_SPABS & " --ccb-ppabs " & CCB_PPABS & " --ccb-ldsize " & CCB_LDSIZE & " --ccb-ldprs " & CCB_LDPRS & " --ccb-ldplut " & CCB_LDPLUT & " --ccb-ccbpre " & CCB_CCBPRE & " --ccb-yoxy " & CCB_YOXY & " --ccb-acsc " & CCB_ACSC & " --ccb-alsc " & CCB_ALSC & " --ccb-acw " & CCB_ACW & " --ccb-accw " & CCB_ACCW & " --ccb-twd " & CCB_TWD & " --ccb-lce " & CCB_LCE & " --ccb-ace " & CCB_ACE & " --ccb-maria " & CCB_MARIA & " --ccb-pxor " & CCB_PXOR & " --ccb-useav " & CCB_USEAV & " --ccb-plutpos " & CCB_PLUTPOS & " --ccb-bgnd " & "set" & " --ccb-noblk " & CCB_NOBLK & " --pre0-literal " & PRE0_LITERAL & " --pre0-bgnd " & PRE0_BGND & " --pre0-rep8 " & PRE0_REP8

            'Made for action prompts while playing
        ElseIf Path.GetDirectoryName(inputfile).ToUpper.Contains("\JYANPAI\MENU") Then
            Argument = "-b " & BPP & " --coded " & CODED & " --lrform " & LRFORM & " --packed " & PACKED & " --transparent " & TRANSPARENT & " --ccb-skip " & CCB_SKIP & " --ccb-last " & CCB_LAST & " --ccb-npabs " & CCB_NPABS & " --ccb-spabs " & CCB_SPABS & " --ccb-ppabs " & CCB_PPABS & " --ccb-ldsize " & CCB_LDSIZE & " --ccb-ldprs " & CCB_LDPRS & " --ccb-ldplut " & CCB_LDPLUT & " --ccb-ccbpre " & CCB_CCBPRE & " --ccb-yoxy " & CCB_YOXY & " --ccb-acsc " & CCB_ACSC & " --ccb-alsc " & CCB_ALSC & " --ccb-acw " & CCB_ACW & " --ccb-accw " & CCB_ACCW & " --ccb-twd " & CCB_TWD & " --ccb-lce " & CCB_LCE & " --ccb-ace " & CCB_ACE & " --ccb-maria " & CCB_MARIA & " --ccb-pxor " & CCB_PXOR & " --ccb-useav " & CCB_USEAV & " --ccb-plutpos " & CCB_PLUTPOS & " --ccb-bgnd " & CCB_BGND & " --ccb-noblk " & CCB_NOBLK & " --pre0-literal " & PRE0_LITERAL & " --pre0-bgnd " & PRE0_BGND & " --pre0-rep8 " & PRE0_REP8


            'Everything else
        Else
            Argument = "-b " & "16" & " --coded " & False & " --lrform " & LRFORM & " --packed " & PACKED & " --transparent " & TRANSPARENT & " --ccb-skip " & CCB_SKIP & " --ccb-last " & CCB_LAST & " --ccb-npabs " & CCB_NPABS & " --ccb-spabs " & CCB_SPABS & " --ccb-ppabs " & CCB_PPABS & " --ccb-ldsize " & CCB_LDSIZE & " --ccb-ldprs " & CCB_LDPRS & " --ccb-ldplut " & CCB_LDPLUT & " --ccb-ccbpre " & CCB_CCBPRE & " --ccb-yoxy " & CCB_YOXY & " --ccb-acsc " & CCB_ACSC & " --ccb-alsc " & CCB_ALSC & " --ccb-acw " & CCB_ACW & " --ccb-accw " & CCB_ACCW & " --ccb-twd " & CCB_TWD & " --ccb-lce " & CCB_LCE & " --ccb-ace " & CCB_ACE & " --ccb-maria " & CCB_MARIA & " --ccb-pxor " & CCB_PXOR & " --ccb-useav " & CCB_USEAV & " --ccb-plutpos " & CCB_PLUTPOS & " --ccb-bgnd " & CCB_BGND & " --ccb-noblk " & CCB_NOBLK & " --pre0-literal " & PRE0_LITERAL & " --pre0-bgnd " & PRE0_BGND & " --pre0-rep8 " & PRE0_REP8
        End If

        Return Argument.ToLower
    End Function


End Class
