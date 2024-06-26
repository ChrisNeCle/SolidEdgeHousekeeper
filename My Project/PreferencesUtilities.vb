﻿Option Strict On
Imports Newtonsoft.Json

Public Class PreferencesUtilities

    Private Function GetPreferencesDirectory() As String
        Dim StartupPath As String = System.Windows.Forms.Application.StartupPath()
        Dim PreferencesDirectory = "Preferences"

        ' Dim Defaults As String() = Nothing
        ' Defaults = IO.File.ReadAllLines(DefaultsFilename)

        Return String.Format("{0}\{1}", StartupPath, PreferencesDirectory)
    End Function

    Private Function GetTaskListPath(CheckExisting As Boolean) As String
        Dim Filename = "task_list.json"
        Dim TaskListPath = String.Format("{0}\{1}", GetPreferencesDirectory, Filename)

        If CheckExisting Then
            If FileIO.FileSystem.FileExists(TaskListPath) Then
                Return TaskListPath
            Else
                Return ""
            End If
        Else
            Return TaskListPath
        End If
    End Function

    Public Sub SaveTaskList(TaskList As List(Of Task))
        Dim tmpJSONDict As New Dictionary(Of String, String)
        Dim JSONString As String

        Dim Outfile = GetTaskListPath(CheckExisting:=False)

        For Each Task As Task In TaskList
            ' To allow copies of a given Task, make the Key Task.Description rather than Task.Name
            tmpJSONDict(Task.Description) = Task.GetFormState()
        Next

        JSONString = JsonConvert.SerializeObject(tmpJSONDict)

        IO.File.WriteAllText(Outfile, JSONString)
    End Sub

    Public Function GetTaskList() As List(Of Task)
        Dim TaskList As New List(Of Task)
        Dim Task As Task
        Dim JSONDict As Dictionary(Of String, String)
        Dim JSONString As String

        Dim TaskJSONDict As Dictionary(Of String, String)
        Dim TaskDescription As String
        Dim TaskName As String

        Dim Infile = GetTaskListPath(CheckExisting:=True)

        If Infile = "" Then
            TaskList = BuildTaskListFromScratch()
        Else
            JSONString = IO.File.ReadAllText(Infile)

            JSONDict = JsonConvert.DeserializeObject(Of Dictionary(Of String, String))(JSONString)

            For Each TaskDescription In JSONDict.Keys
                JSONString = JSONDict(TaskDescription)
                TaskJSONDict = JsonConvert.DeserializeObject(Of Dictionary(Of String, String))(JSONString)
                TaskName = TaskJSONDict("TaskName")

                Task = GetNewTaskInstance(TaskName, TaskDescription)
                If Task IsNot Nothing Then
                    TaskList.Add(Task)
                End If
            Next

        End If


        Return TaskList
    End Function



    Public Function GetNewTaskInstance(
        TaskName As String,
        TaskDescription As String
        ) As Task

        Dim Task As Task = Nothing

        Dim tmpTaskList = BuildTaskListFromScratch()

        For Each Task In tmpTaskList
            If Task.Name = TaskName Then
                Task.Description = TaskDescription
                Exit For
            End If
        Next

        Return Task
    End Function

    Public Sub ConfigureTaskUI(Task As Task)
        Dim JSONDict As Dictionary(Of String, String)
        Dim JSONString As String

        'Dim TaskJSONDict As Dictionary(Of String, String)
        Dim TaskJSONString As String

        Dim Infile = GetTaskListPath(CheckExisting:=True)

        If Not Infile = "" Then
            JSONString = IO.File.ReadAllText(Infile)

            JSONDict = JsonConvert.DeserializeObject(Of Dictionary(Of String, String))(JSONString)

            TaskJSONString = JSONDict(Task.Description)

            Task.SetFormState(TaskJSONString)
        End If
    End Sub

    Public Function BuildTaskListFromScratch() As List(Of Task)
        Dim TaskList As New List(Of Task)
        Dim KnownTasks As New List(Of String)

        ' Update
        TaskList.Add(New TaskOpenSave)
        TaskList.Add(New TaskActivateAndUpdateAll)
        TaskList.Add(New TaskUpdateMaterialFromMaterialTable)
        TaskList.Add(New TaskUpdatePartCopies)
        TaskList.Add(New TaskUpdatePhysicalProperties)
        TaskList.Add(New TaskUpdateModelSizeInVariableTable)
        TaskList.Add(New TaskUpdateDesignForCost)
        TaskList.Add(New TaskUpdateDrawingViews)
        TaskList.Add(New TaskRegenerateFlatModel)

        ' Edit
        TaskList.Add(New TaskEditProperties)
        TaskList.Add(New TaskEditVariables)
        TaskList.Add(New TaskEditInteractively)
        TaskList.Add(New TaskBreakPartCopyLinks)

        ' Appearance
        TaskList.Add(New TaskUpdateModelStylesFromTemplate)
        TaskList.Add(New TaskUpdateDrawingStylesFromTemplate)
        TaskList.Add(New TaskRemoveFaceStyleOverrides)
        TaskList.Add(New TaskHideConstructions)
        TaskList.Add(New TaskFitView)

        ' Check
        TaskList.Add(New TaskCheckInterference)
        TaskList.Add(New TaskCheckLinks)
        TaskList.Add(New TaskCheckRelationships)
        TaskList.Add(New TaskCheckFlatPattern)
        TaskList.Add(New TaskCheckMaterialNotInMaterialTable)
        TaskList.Add(New TaskCheckMissingDrawing)
        TaskList.Add(New TaskCheckPartNumberDoesNotMatchFilename)
        TaskList.Add(New TaskCheckPartCopies)
        TaskList.Add(New TaskCheckDrawingPartsList)
        TaskList.Add(New TaskCheckDrawings)

        ' External
        TaskList.Add(New TaskRunExternalProgram)

        ' Output
        TaskList.Add(New TaskSaveModelAs)
        TaskList.Add(New TaskSaveDrawingAs)
        TaskList.Add(New TaskPrint)

        For Each Task As Task In TaskList
            Task.RememberTaskSelections = Form1.RememberTaskSelections
            KnownTasks.Add(Task.Name.ToLower)
        Next

        CheckForUnknownTasks(KnownTasks)

        'Dim tmpTaskList As New List(Of Task)
        'For Each Task In TaskList
        '    If Task.HasOptions Then tmpTaskList.Add(Task)
        'Next

        'Return tmpTaskList

        Return TaskList
    End Function

    Private Sub CheckForUnknownTasks(KnownTasks As List(Of String))
        Dim HardcodedPath = "C:\data\CAD\scripts\SolidEdgeHousekeeper\My Project"
        Dim Filenames As List(Of String)
        Dim Filename As String

        Dim UnknownTasks As New List(Of String)

        Dim tf As Boolean
        Dim s As String = String.Format("Unknown Tasks{0}", vbCrLf)

        If FileIO.FileSystem.DirectoryExists(HardcodedPath) Then
            Filenames = IO.Directory.GetFiles(HardcodedPath).ToList

            For Each Filename In Filenames
                Filename = System.IO.Path.GetFileNameWithoutExtension(Filename).ToLower
                tf = Filename.StartsWith("task")
                tf = tf And Not Filename = "task"
                tf = tf And Not Filename.StartsWith("task_")
                tf = tf And Not Filename.EndsWith(".aux")
                tf = tf And Not KnownTasks.Contains(Filename)

                If tf Then
                    UnknownTasks.Add(Filename)
                End If
            Next

            If UnknownTasks.Count > 0 Then
                For Each UnknownTask As String In UnknownTasks
                    s = String.Format("{0}{1}{2}", s, UnknownTask, vbCrLf)
                Next
                MsgBox(s)
            End If

        End If
    End Sub

    Public Sub CreatePreferencesFolder()
        Dim StartupPath As String = System.Windows.Forms.Application.StartupPath()
        Dim PreferencesFolder As String = String.Format("{0}\Preferences", StartupPath)
        If Not FileIO.FileSystem.DirectoryExists(PreferencesFolder) Then
            FileIO.FileSystem.CreateDirectory(PreferencesFolder)
        End If
    End Sub

    Public Sub CreateFilenameCharmap()
        Dim FCD As New FilenameCharmapDoctor()  ' Creates the file filename_charmap.txt if it does not exist.
    End Sub

End Class
