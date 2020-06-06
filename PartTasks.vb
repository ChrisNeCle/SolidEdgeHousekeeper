﻿Option Strict On

Imports SolidEdgeCommunity

Public Class PartTasks
    Inherits IsolatedTaskProxy

    Public Function FailedOrWarnedFeatures(
        ByVal SEDoc As SolidEdgeFramework.SolidEdgeDocument,
        ByVal Configuration As Dictionary(Of String, String),
        ByVal SEApp As SolidEdgeFramework.Application
        ) As Dictionary(Of Integer, List(Of String))

        Dim ErrorMessage As New Dictionary(Of Integer, List(Of String))

        ErrorMessage = InvokeSTAThread(
                               Of SolidEdgePart.PartDocument,
                               Dictionary(Of String, String),
                               SolidEdgeFramework.Application,
                               Dictionary(Of Integer, List(Of String)))(
                                   AddressOf FailedOrWarnedFeaturesInternal,
                                   CType(SEDoc, SolidEdgePart.PartDocument),
                                   Configuration,
                                   SEApp)

        Return ErrorMessage

    End Function

    Private Function FailedOrWarnedFeaturesInternal(
        ByVal SEDoc As SolidEdgePart.PartDocument,
        ByVal Configuration As Dictionary(Of String, String),
        ByVal SEApp As SolidEdgeFramework.Application
        ) As Dictionary(Of Integer, List(Of String))

        Dim ErrorMessageList As New List(Of String)
        Dim ExitStatus As Integer = 0
        Dim ErrorMessage As New Dictionary(Of Integer, List(Of String))

        Dim Models As SolidEdgePart.Models
        Dim Model As SolidEdgePart.Model
        Dim Features As SolidEdgePart.Features
        Dim FeatureName As String
        Dim Status As SolidEdgePart.FeatureStatusConstants

        Dim TF As Boolean
        Dim FeatureSystemNames As New List(Of String)
        Dim FeatureSystemName As String

        Models = SEDoc.Models

        If (Models.Count > 0) And (Models.Count < 300) Then
            For Each Model In Models
                Features = Model.Features
                For Each Feature In Features


                    'Dim FeatureType = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetType(Feature)
                    'ExitStatus = 1
                    'ErrorMessageList.Add(String.Format("{0}", FeatureType.ToString))
                    'If FeatureType.ToString = "SolidEdgePart.ExtrudedProtrusion" Then
                    '    Dim FaceStyle As SolidEdgeFramework.FaceStyle = CType(FeatureType, SolidEdgePart.ExtrudedProtrusion).GetStyle()
                    'End If



                    'FeatureSystemName = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetPropertyValue(Of String)(Feature, "SystemName")
                    FeatureSystemName = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetPropertyValue(Of String)(Feature, "Name")

                    If Not FeatureSystemNames.Contains(FeatureSystemName) Then
                        FeatureSystemNames.Add(FeatureSystemName)

                        'Some Sync part features don't have a Status field.
                        Try
                            FeatureName = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetPropertyValue(Of String)(Feature, "Name")

                            Status = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetPropertyValue(
                            Of SolidEdgePart.FeatureStatusConstants)(Feature, "Status", CType(0, SolidEdgePart.FeatureStatusConstants))

                            TF = Status = SolidEdgePart.FeatureStatusConstants.igFeatureFailed
                            TF = TF Or Status = SolidEdgePart.FeatureStatusConstants.igFeatureWarned
                            If TF Then
                                ExitStatus = 1
                                ErrorMessageList.Add(FeatureName)
                            End If

                        Catch ex As Exception

                        End Try
                    End If

                Next
            Next
        ElseIf Models.Count >= 300 Then
            ExitStatus = 1
            ErrorMessageList.Add(String.Format("{0} models exceeds maximum to process", Models.Count.ToString))
        End If

        ErrorMessage(ExitStatus) = ErrorMessageList
        Return ErrorMessage
    End Function


    Public Function SuppressedOrRolledBackFeatures(
        ByVal SEDoc As SolidEdgeFramework.SolidEdgeDocument,
        ByVal Configuration As Dictionary(Of String, String),
        ByVal SEApp As SolidEdgeFramework.Application
        ) As Dictionary(Of Integer, List(Of String))

        Dim ErrorMessage As New Dictionary(Of Integer, List(Of String))

        ErrorMessage = InvokeSTAThread(
                               Of SolidEdgePart.PartDocument,
                               Dictionary(Of String, String),
                               SolidEdgeFramework.Application,
                               Dictionary(Of Integer, List(Of String)))(
                                   AddressOf SuppressedOrRolledBackFeaturesInternal,
                                   CType(SEDoc, SolidEdgePart.PartDocument),
                                   Configuration,
                                   SEApp)

        Return ErrorMessage

    End Function

    Private Function SuppressedOrRolledBackFeaturesInternal(
        ByVal SEDoc As SolidEdgePart.PartDocument,
        ByVal Configuration As Dictionary(Of String, String),
        ByVal SEApp As SolidEdgeFramework.Application
        ) As Dictionary(Of Integer, List(Of String))

        Dim ErrorMessageList As New List(Of String)
        Dim ExitStatus As Integer = 0
        Dim ErrorMessage As New Dictionary(Of Integer, List(Of String))

        Dim Models As SolidEdgePart.Models
        Dim Model As SolidEdgePart.Model
        Dim Features As SolidEdgePart.Features
        Dim FeatureName As String
        Dim Status As SolidEdgePart.FeatureStatusConstants

        Dim TF As Boolean
        Dim FeatureSystemNames As New List(Of String)
        Dim FeatureSystemName As String

        Models = SEDoc.Models

        If (Models.Count > 0) And (Models.Count < 300) Then
            For Each Model In Models
                Features = Model.Features
                For Each Feature In Features
                    FeatureSystemName = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetPropertyValue(Of String)(Feature, "SystemName")

                    If Not FeatureSystemNames.Contains(FeatureSystemName) Then
                        FeatureSystemNames.Add(FeatureSystemName)

                        'Some Sync part features don't have a Status field.
                        Try
                            FeatureName = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetPropertyValue(Of String)(Feature, "Name")

                            Status = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetPropertyValue(
                            Of SolidEdgePart.FeatureStatusConstants)(Feature, "Status", CType(0, SolidEdgePart.FeatureStatusConstants))

                            TF = Status = SolidEdgePart.FeatureStatusConstants.igFeatureSuppressed
                            TF = TF Or Status = SolidEdgePart.FeatureStatusConstants.igFeatureRolledBack
                            If TF Then
                                ExitStatus = 1
                                ErrorMessageList.Add(FeatureName)
                            End If

                        Catch ex As Exception

                        End Try
                    End If
                Next
            Next
        ElseIf Models.Count >= 300 Then
            ExitStatus = 1
            ErrorMessageList.Add(String.Format("{0} models exceeds maximum to process", Models.Count.ToString))
        End If

        ErrorMessage(ExitStatus) = ErrorMessageList
        Return ErrorMessage
    End Function


    Public Function UnderconstrainedProfiles(
        ByVal SEDoc As SolidEdgeFramework.SolidEdgeDocument,
        ByVal Configuration As Dictionary(Of String, String),
        ByVal SEApp As SolidEdgeFramework.Application
        ) As Dictionary(Of Integer, List(Of String))

        Dim ErrorMessage As New Dictionary(Of Integer, List(Of String))

        ErrorMessage = InvokeSTAThread(
                               Of SolidEdgePart.PartDocument,
                               Dictionary(Of String, String),
                               SolidEdgeFramework.Application,
                               Dictionary(Of Integer, List(Of String)))(
                                   AddressOf UnderconstrainedProfilesInternal,
                                   CType(SEDoc, SolidEdgePart.PartDocument),
                                   Configuration,
                                   SEApp)

        Return ErrorMessage

    End Function

    Private Function UnderconstrainedProfilesInternal(
        ByVal SEDoc As SolidEdgePart.PartDocument,
        ByVal Configuration As Dictionary(Of String, String),
        ByVal SEApp As SolidEdgeFramework.Application
        ) As Dictionary(Of Integer, List(Of String))

        Dim ErrorMessageList As New List(Of String)
        Dim ExitStatus As Integer = 0
        Dim ErrorMessage As New Dictionary(Of Integer, List(Of String))

        Dim ProfileSets As SolidEdgePart.ProfileSets = SEDoc.ProfileSets
        Dim ProfileSet As SolidEdgePart.ProfileSet

        ' Not applicable in sync models.
        If SEDoc.ModelingMode.ToString = "seModelingModeOrdered" Then
            For Each ProfileSet In ProfileSets
                If ProfileSet.IsUnderDefined Then
                    ExitStatus = 1
                End If
            Next
        End If

        ErrorMessage(ExitStatus) = ErrorMessageList
        Return ErrorMessage
    End Function


    Public Function InsertPartCopiesOutOfDate(
        ByVal SEDoc As SolidEdgeFramework.SolidEdgeDocument,
        ByVal Configuration As Dictionary(Of String, String),
        ByVal SEApp As SolidEdgeFramework.Application
        ) As Dictionary(Of Integer, List(Of String))

        Dim ErrorMessage As New Dictionary(Of Integer, List(Of String))

        ErrorMessage = InvokeSTAThread(
                               Of SolidEdgePart.PartDocument,
                               Dictionary(Of String, String),
                               SolidEdgeFramework.Application,
                               Dictionary(Of Integer, List(Of String)))(
                                   AddressOf InsertPartCopiesOutOfDateInternal,
                                   CType(SEDoc, SolidEdgePart.PartDocument),
                                   Configuration,
                                   SEApp)

        Return ErrorMessage

    End Function

    Private Function InsertPartCopiesOutOfDateInternal(
        ByVal SEDoc As SolidEdgePart.PartDocument,
        ByVal Configuration As Dictionary(Of String, String),
        ByVal SEApp As SolidEdgeFramework.Application
        ) As Dictionary(Of Integer, List(Of String))

        Dim ErrorMessageList As New List(Of String)
        Dim ExitStatus As Integer = 0
        Dim ErrorMessage As New Dictionary(Of Integer, List(Of String))

        Dim Models As SolidEdgePart.Models
        Dim Model As SolidEdgePart.Model
        Dim CopiedParts As SolidEdgePart.CopiedParts
        Dim CopiedPart As SolidEdgePart.CopiedPart

        Models = SEDoc.Models

        Dim TF As Boolean

        If (Models.Count > 0) And (Models.Count < 300) Then
            For Each Model In Models
                CopiedParts = Model.CopiedParts
                If CopiedParts.Count > 0 Then
                    For Each CopiedPart In CopiedParts
                        TF = FileIO.FileSystem.FileExists(CopiedPart.FileName)
                        TF = TF Or (CopiedPart.FileName = "")  ' Implies no link to outside file
                        TF = TF And CopiedPart.IsUpToDate
                        If Not TF Then
                            ExitStatus = 1
                            ErrorMessageList.Add(CopiedPart.Name)
                        End If
                    Next
                End If
            Next
        ElseIf Models.Count >= 300 Then
            ExitStatus = 1
            ErrorMessageList.Add(String.Format("{0} models exceeds maximum to process", Models.Count.ToString))
        End If

        ErrorMessage(ExitStatus) = ErrorMessageList
        Return ErrorMessage
    End Function


    Public Function MaterialNotInMaterialTable(
        ByVal SEDoc As SolidEdgeFramework.SolidEdgeDocument,
        ByVal Configuration As Dictionary(Of String, String),
        ByVal SEApp As SolidEdgeFramework.Application
        ) As Dictionary(Of Integer, List(Of String))

        Dim ErrorMessage As New Dictionary(Of Integer, List(Of String))

        ErrorMessage = InvokeSTAThread(
                               Of SolidEdgePart.PartDocument,
                               Dictionary(Of String, String),
                               SolidEdgeFramework.Application,
                               Dictionary(Of Integer, List(Of String)))(
                                   AddressOf MaterialNotInMaterialTableInternal,
                                   CType(SEDoc, SolidEdgePart.PartDocument),
                                   Configuration,
                                   SEApp)

        Return ErrorMessage

    End Function

    Private Function MaterialNotInMaterialTableInternal(
        ByVal SEDoc As SolidEdgePart.PartDocument,
        ByVal Configuration As Dictionary(Of String, String),
        ByVal SEApp As SolidEdgeFramework.Application
        ) As Dictionary(Of Integer, List(Of String))

        Dim ErrorMessageList As New List(Of String)
        Dim ExitStatus As Integer = 0
        Dim ErrorMessage As New Dictionary(Of Integer, List(Of String))

        Dim MatTable As SolidEdgeFramework.MatTable

        Dim MaterialLibList As Object = Nothing
        Dim NumMaterialLibraries As Integer
        Dim MaterialList As Object = Nothing
        Dim NumMaterials As Integer

        Dim ActiveMaterialLibrary As String = System.IO.Path.GetFileNameWithoutExtension(Configuration("TextBoxActiveMaterialLibrary"))
        Dim ActiveMaterialLibraryPresent As Boolean = False
        Dim CurrentMaterialName As String = ""
        Dim MatTableMaterial As Object
        Dim CurrentMaterialNameInLibrary As Boolean = False
        Dim CurrentMaterialMatchesLibMaterial As Boolean = True

        Dim msg As String = ""

        Dim Models As SolidEdgePart.Models

        Models = SEDoc.Models

        If Models.Count > 0 Then

            MatTable = SEApp.GetMaterialTable()
            MatTable.GetCurrentMaterialName(SEDoc, CurrentMaterialName)
            MatTable.GetMaterialLibraryList(MaterialLibList, NumMaterialLibraries)

            'Make sure the ActiveMaterialLibrary in settings.txt is present
            For Each MatTableMaterial In CType(MaterialLibList, System.Array)
                If MatTableMaterial.ToString = ActiveMaterialLibrary Then
                    ActiveMaterialLibraryPresent = True
                    Exit For
                End If
            Next

            If Not ActiveMaterialLibraryPresent Then
                msg = "ActiveMaterialLibrary " + Configuration("TextBoxActiveMaterialLibrary") + " not found.  Exiting..." + Chr(13)
                msg += "Please update the Material Table on the Configuration tab." + Chr(13)
                MsgBox(msg)
                SEApp.Quit()
                End
            End If

            'See if the CurrentMaterialName is in the ActiveLibrary
            MatTable.GetMaterialListFromLibrary(ActiveMaterialLibrary, NumMaterials, MaterialList)
            For Each MatTableMaterial In CType(MaterialList, System.Array)
                If MatTableMaterial.ToString.ToLower.Trim = CurrentMaterialName.ToLower.Trim Then
                    CurrentMaterialNameInLibrary = True
                    Exit For
                End If
            Next

            If Not CurrentMaterialNameInLibrary Then
                ExitStatus = 1
                If CurrentMaterialName = "" Then
                    ErrorMessageList.Add(String.Format("Material 'None' not in {0}", ActiveMaterialLibrary))
                Else
                    ErrorMessageList.Add(String.Format("Material '{0}' not in {1}", CurrentMaterialName, ActiveMaterialLibrary))
                End If
            End If
        End If

        ErrorMessage(ExitStatus) = ErrorMessageList
        Return ErrorMessage
    End Function


    Public Function PartNumberDoesNotMatchFilename(
        ByVal SEDoc As SolidEdgeFramework.SolidEdgeDocument,
        ByVal Configuration As Dictionary(Of String, String),
        ByVal SEApp As SolidEdgeFramework.Application
        ) As Dictionary(Of Integer, List(Of String))

        Dim ErrorMessage As New Dictionary(Of Integer, List(Of String))

        ErrorMessage = InvokeSTAThread(
                               Of SolidEdgePart.PartDocument,
                               Dictionary(Of String, String),
                               SolidEdgeFramework.Application,
                               Dictionary(Of Integer, List(Of String)))(
                                   AddressOf PartNumberDoesNotMatchFilenameInternal,
                                   CType(SEDoc, SolidEdgePart.PartDocument),
                                   Configuration,
                                   SEApp)

        Return ErrorMessage

    End Function

    Private Function PartNumberDoesNotMatchFilenameInternal(
        ByVal SEDoc As SolidEdgePart.PartDocument,
        ByVal Configuration As Dictionary(Of String, String),
        ByVal SEApp As SolidEdgeFramework.Application
        ) As Dictionary(Of Integer, List(Of String))

        Dim ErrorMessageList As New List(Of String)
        Dim ExitStatus As Integer = 0
        Dim ErrorMessage As New Dictionary(Of Integer, List(Of String))

        Dim PropertySets As SolidEdgeFramework.PropertySets = Nothing
        Dim Properties As SolidEdgeFramework.Properties = Nothing
        Dim Prop As SolidEdgeFramework.Property = Nothing

        Dim PartNumber As String = ""
        Dim PartNumberPropertyFound As Boolean = False
        Dim TF As Boolean
        Dim Filename As String

        'Get the bare file name without directory information
        Filename = System.IO.Path.GetFileName(SEDoc.FullName)

        Dim msg As String = ""

        PropertySets = CType(SEDoc.Properties, SolidEdgeFramework.PropertySets)

        For Each Properties In PropertySets
            msg += Properties.Name + Chr(13)
            For Each Prop In Properties
                TF = (Configuration("ComboBoxPartNumberPropertySet").ToLower = "custom") And (Properties.Name.ToLower = "custom")
                If TF Then
                    If Prop.Name = Configuration("TextBoxPartNumberPropertyName") Then
                        'If Prop.Name = TextBoxPartNumberPropertyName.Text Then
                        PartNumber = CType(Prop.Value, String).Trim
                        PartNumberPropertyFound = True
                        Exit For
                    End If
                Else
                    If Prop.Name = Configuration("TextBoxPartNumberPropertyName") Then
                        PartNumber = CType(Prop.Value, String).Trim
                        PartNumberPropertyFound = True
                        Exit For
                    End If
                End If
            Next
            If PartNumberPropertyFound Then
                Exit For
            End If
        Next

        If PartNumberPropertyFound Then
            If PartNumber.Trim = "" Then
                ExitStatus = 1
                ErrorMessageList.Add("Part number not assigned")
            End If
            If Not Filename.Contains(PartNumber) Then
                ExitStatus = 1
                ErrorMessageList.Add(String.Format("Part number '{0}' not found in filename '{1}'", PartNumber, Filename))
            End If
        Else
            ExitStatus = 1
            ErrorMessageList.Add(String.Format("Property name: '{0}' not found in property set: '{1}'",
                                     Configuration("TextBoxPartNumberPropertyName"),
                                     Configuration("ComboBoxPartNumberPropertySet")))
            If Configuration("TextBoxPartNumberPropertyName") = "" Then
                ErrorMessageList.Add("Check the Configuration tab for valid entries")
            End If
        End If

        ErrorMessage(ExitStatus) = ErrorMessageList
        Return ErrorMessage
    End Function


    Public Function UpdateInsertPartCopies(
        ByVal SEDoc As SolidEdgeFramework.SolidEdgeDocument,
        ByVal Configuration As Dictionary(Of String, String),
        ByVal SEApp As SolidEdgeFramework.Application
        ) As Dictionary(Of Integer, List(Of String))

        Dim ErrorMessage As New Dictionary(Of Integer, List(Of String))

        ErrorMessage = InvokeSTAThread(
                               Of SolidEdgePart.PartDocument,
                               Dictionary(Of String, String),
                               SolidEdgeFramework.Application,
                               Dictionary(Of Integer, List(Of String)))(
                                   AddressOf UpdateInsertPartCopiesInternal,
                                   CType(SEDoc, SolidEdgePart.PartDocument),
                                   Configuration,
                                   SEApp)

        Return ErrorMessage

    End Function

    Private Function UpdateInsertPartCopiesInternal(
        ByVal SEDoc As SolidEdgePart.PartDocument,
        ByVal Configuration As Dictionary(Of String, String),
        ByVal SEApp As SolidEdgeFramework.Application
        ) As Dictionary(Of Integer, List(Of String))

        Dim ErrorMessageList As New List(Of String)
        Dim ExitStatus As Integer = 0
        Dim ErrorMessage As New Dictionary(Of Integer, List(Of String))

        Dim Models As SolidEdgePart.Models
        Dim Model As SolidEdgePart.Model
        Dim CopiedParts As SolidEdgePart.CopiedParts
        Dim CopiedPart As SolidEdgePart.CopiedPart

        Models = SEDoc.Models

        Dim TF As Boolean

        If (Models.Count > 0) And (Models.Count < 300) Then
            For Each Model In Models
                CopiedParts = Model.CopiedParts
                If CopiedParts.Count > 0 Then
                    For Each CopiedPart In CopiedParts
                        TF = FileIO.FileSystem.FileExists(CopiedPart.FileName)
                        TF = TF Or (CopiedPart.FileName = "")  ' Implies no link to outside file
                        If Not TF Then
                            ExitStatus = 1
                            ErrorMessageList.Add(String.Format("Insert part copy file not found: {0}", CopiedPart.FileName))
                        ElseIf Not CopiedPart.IsUpToDate Then
                            CopiedPart.Update()
                            If SEDoc.ReadOnly Then
                                ExitStatus = 1
                                ErrorMessageList.Add("Cannot save document marked 'Read Only'")
                            Else
                                SEDoc.Save()
                                SEApp.DoIdle()
                                ExitStatus = 1
                                ErrorMessageList.Add(String.Format("Updated insert part copy: {0}", CopiedPart.Name))
                            End If
                        End If
                    Next
                End If
            Next
        ElseIf Models.Count >= 300 Then
            ExitStatus = 1
            ErrorMessageList.Add(String.Format("{0} models exceeds maximum to process", Models.Count.ToString))
        End If

        ErrorMessage(ExitStatus) = ErrorMessageList
        Return ErrorMessage
    End Function


    Public Function UpdateMaterialFromMaterialTable(
        ByVal SEDoc As SolidEdgeFramework.SolidEdgeDocument,
        ByVal Configuration As Dictionary(Of String, String),
        ByVal SEApp As SolidEdgeFramework.Application
        ) As Dictionary(Of Integer, List(Of String))

        Dim ErrorMessage As New Dictionary(Of Integer, List(Of String))

        ErrorMessage = InvokeSTAThread(
                               Of SolidEdgePart.PartDocument,
                               Dictionary(Of String, String),
                               SolidEdgeFramework.Application,
                               Dictionary(Of Integer, List(Of String)))(
                                   AddressOf UpdateMaterialFromMaterialTableInternal,
                                   CType(SEDoc, SolidEdgePart.PartDocument),
                                   Configuration,
                                   SEApp)

        Return ErrorMessage

    End Function

    Private Function UpdateMaterialFromMaterialTableInternal(
        ByVal SEDoc As SolidEdgePart.PartDocument,
        ByVal Configuration As Dictionary(Of String, String),
        ByVal SEApp As SolidEdgeFramework.Application
        ) As Dictionary(Of Integer, List(Of String))

        'DESCRIPTION
        'Sometext
        'Dim MatTable As SolidEdgeFramework.MatTable

        'Dim MaterialLibList As Object = Nothing
        'Dim NumMaterialLibraries As Integer
        'Dim MaterialList As Object = Nothing
        'Dim NumMaterials As Integer

        'Dim ActiveMaterialLibrary As String = System.IO.Path.GetFileNameWithoutExtension(Configuration("TextBoxActiveMaterialLibrary"))
        'Dim CurrentMaterialName As String = ""
        'Dim MatTableMaterial As Object

        'Dim Models As SolidEdgePart.Models = SEDoc.Models


        'Dim IsActiveMaterialLibraryPresent As Func(Of Object, String, Boolean) =
        '    Function(_MaterialLibList, _ActiveMaterialLibrary)
        '        For Each MatTableLibrary In CType(_MaterialLibList, System.Array)
        '            If MatTableLibrary.ToString = _ActiveMaterialLibrary Then
        '                Return True
        '                Exit For
        '            End If
        '        Next

        '        Return False
        '    End Function

        'Dim CurrentMaterialNameInLibrary As Func(Of String, Object, Boolean) =
        '    Function(_CurrentMaterialName, _MaterialList)
        '        For Each MatTableMaterial In CType(_MaterialList, System.Array)
        '            If MatTableMaterial.ToString.ToLower.Trim = _CurrentMaterialName.ToLower.Trim Then
        '                Return True
        '            End If
        '        Next
        '        Return False
        '    End Function

        'Dim CloseEnough As Func(Of Double, Double, Boolean) =
        '    Function(_DocPropValue, _LibPropValue)
        '        Dim DPV As Double = _DocPropValue
        '        Dim LPV As Double = _LibPropValue

        '        If Not ((DPV = 0) And (LPV = 0)) Then  ' Avoid divide by 0.  Anyway, they match.
        '            Dim NormalizedDifference As Double = (DPV - LPV) / (DPV + LPV) / 2
        '            If Not Math.Abs(NormalizedDifference) < 0.001 Then
        '                Return False
        '            End If
        '        End If

        '        Return True
        '    End Function

        'Dim MaterialPropertiesMatch As Func(Of SolidEdgeFramework.MatTable, Object, Boolean) =
        '    Function(_MatTable, _MatTableMaterial)
        '        Dim MatTableProps As Array = System.Enum.GetValues(GetType(SolidEdgeConstants.MatTablePropIndex))
        '        Dim MatTableProp As SolidEdgeFramework.MatTablePropIndex
        '        Dim DocPropValue As Object = Nothing
        '        Dim LibPropValue As Object = Nothing

        '        For Each MatTableProp In MatTableProps
        '            ' This function populates 'LibPropValue'
        '            _MatTable.GetMaterialPropValueFromLibrary(_MatTableMaterial.ToString, ActiveMaterialLibrary, MatTableProp, LibPropValue)

        '            ' This function populates 'DocPropValue'
        '            _MatTable.GetMaterialPropValueFromDoc(SEDoc, MatTableProp, DocPropValue)

        '            ' MatTableProps are either Double or String.
        '            If (DocPropValue.GetType = GetType(Double)) And (LibPropValue.GetType = GetType(Double)) Then
        '                ' Double types may have insignificant differences.
        '                If Not CloseEnough(CType(DocPropValue, Double), CType(LibPropValue, Double)) Then
        '                    Return False
        '                End If
        '            Else
        '                If CType(DocPropValue, String) <> CType(LibPropValue, String) Then
        '                    Return False
        '                End If
        '            End If
        '            DocPropValue = Nothing
        '            LibPropValue = Nothing
        '        Next

        '        Return True
        '    End Function

        'Dim CurrentMaterialFaceStyle As Func(Of SolidEdgeFramework.MatTable, Object, SolidEdgeFramework.FaceStyle) =
        '    Function(_MatTable, _MatTableMaterial)
        '        Dim MatTableProps As Array = System.Enum.GetValues(GetType(SolidEdgeConstants.MatTablePropIndex))
        '        Dim LibPropValue As Object = Nothing
        '        Dim MatTableProp As SolidEdgeFramework.MatTablePropIndex
        '        Dim FaceStyle As SolidEdgeFramework.FaceStyle

        '        For Each MatTableProp In MatTableProps
        '            ' This function populates 'LibPropValue'
        '            _MatTable.GetMaterialPropValueFromLibrary(_MatTableMaterial.ToString, ActiveMaterialLibrary, MatTableProp, LibPropValue)

        '            'MsgBox(String.Format("{0} {1}", MatTableProp.ToString, LibPropValue.ToString))
        '            If MatTableProp.ToString = "seFaceStyle" Then
        '                For Each FaceStyle In CType(SEDoc.FaceStyles, SolidEdgeFramework.FaceStyles)
        '                    If FaceStyle.StyleName = LibPropValue.ToString Then
        '                        Return FaceStyle
        '                    End If
        '                Next

        '            End If

        '            LibPropValue = Nothing
        '        Next
        '        Return Nothing
        '    End Function

        'Dim GetFeatureFaceStyle As Func(Of Object, Dictionary(Of Integer, SolidEdgeFramework.FaceStyle)) =
        '    Function(Feature As Object) As Dictionary(Of Integer, SolidEdgeFramework.FaceStyle)
        '        Dim FeatureFaces As SolidEdgeGeometry.Faces = Nothing
        '        Dim Face As SolidEdgeGeometry.Face
        '        Dim FeatureFaceStyle As SolidEdgeFramework.FaceStyle
        '        Dim FeatureFaceOverrides As New Dictionary(Of Integer, SolidEdgeFramework.FaceStyle)

        '        Dim PopulateFeatureFaceOverrides As Func(Of
        '            SolidEdgeGeometry.Faces,
        '            SolidEdgeFramework.FaceStyle,
        '            Dictionary(Of Integer, SolidEdgeFramework.FaceStyle)) =
        '                Function(FeatureFaces_ As SolidEdgeGeometry.Faces,
        '                         FeatureFaceStyle_ As SolidEdgeFramework.FaceStyle) As Dictionary(Of Integer, SolidEdgeFramework.FaceStyle)
        '                    For Each Face In FeatureFaces_
        '                        FeatureFaceOverrides(Face.ID) = FeatureFaceStyle_
        '                    Next
        '                    Return FeatureFaceOverrides
        '                End Function

        '        Dim FeatureType = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetPropertyValue(
        '        Of SolidEdgePart.FeatureTypeConstants)(Feature, "Type", CType(0, SolidEdgePart.FeatureTypeConstants))

        '        Select Case FeatureType

        '            Case SolidEdgePart.FeatureTypeConstants.igBeadFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.Bead)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll), SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igBendFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.Bend)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll), SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igBlueSurfFeatureObject
        '                ' BlueSurf objects do not have a FaceStyle override.

        '            Case SolidEdgePart.FeatureTypeConstants.igBodyFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.BodyFeature)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                                   SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igBooleanFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.BooleanFeature)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igBreakCornerFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.BreakCorner)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igChamferFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.Chamfer)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igCloseCornerFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.CloseCorner)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igContourFlangeFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.ContourFlange)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igCopiedPartFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.CopiedPart)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    ' Some CopiedParts do not have Faces
        '                    Try
        '                        FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                        Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                    Catch ex As Exception
        '                    End Try
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igCopyConstructionObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.CopyConstruction)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igDeleteBlendFeatureObject 'GetType(SolidEdgePart.DeleteBlend)
        '                Dim Feature_ = CType(Feature, SolidEdgePart.DeleteBlend)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igDeleteFaceFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.DeleteFace)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igDeleteHoleFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.DeleteHole)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igDimpleFeatureObject 'GetType(SolidEdgePart.Dimple)
        '                Dim Feature_ = CType(Feature, SolidEdgePart.Dimple)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igDraftFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.Draft)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igDrawnCutoutFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.DrawnCutout)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igExtrudedCutoutFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.ExtrudedCutout)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igExtrudedProtrusionFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.ExtrudedProtrusion)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igExtrudedSurfaceObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.ExtrudedSurface)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igFaceMoveObject
        '                ' Does not have a Faces collection.

        '            Case SolidEdgePart.FeatureTypeConstants.igFlangeFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.Flange)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igHelixCutoutFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.HelixCutout)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igHelixProtrusionFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.HelixProtrusion)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igHoleFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.Hole)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igJogFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.Jog)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igLipFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.Lip)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igLoftedCutoutFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.LoftedCutout)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igLoftedFlangeFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.LoftedFlange)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igLoftedProtrusionFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.LoftedProtrusion)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igLouverFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.Louver)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igMidSurfaceObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.MidSurface)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igMirrorCopyFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.MirrorCopy)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igMirrorPartFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.MirrorPart)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igNormalCutoutFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.NormalCutout)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igPartingSplitFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.PartingSplit)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igPatternFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.Pattern)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igPatternPartFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.PatternPart)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igRebendFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.Rebend)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igReplaceFaceFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.ReplaceFace)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igResizeHoleObject
        '                ' Does not have a Faces collection.

        '            Case SolidEdgePart.FeatureTypeConstants.igRevolvedCutoutFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.RevolvedCutout)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igRevolvedProtrusionFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.RevolvedProtrusion)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igRibFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.Rib)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igRoundFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.Round)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igSlotFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.Slot)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igSplitFaceObject
        '                ' Does not have a Faces collection.

        '            Case SolidEdgePart.FeatureTypeConstants.igSweptCutoutFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.SweptCutout)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igSweptProtrusionFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.SweptProtrusion)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igTabFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.Tab)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igThickenFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.Thicken)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igThinwallFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.Thinwall)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igTubeFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.TubeFeature)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igUnbendFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.Unbend)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igUserDefinedPatternFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.UserDefinedPattern)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case SolidEdgePart.FeatureTypeConstants.igWebNetworkFeatureObject
        '                Dim Feature_ = CType(Feature, SolidEdgePart.WebNetwork)
        '                FeatureFaceStyle = Feature_.GetStyle()
        '                If FeatureFaceStyle IsNot Nothing Then
        '                    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                           SolidEdgeGeometry.Faces)
        '                    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                End If

        '            Case Else
        '                Select Case FeatureType.ToString
        '                    Case "0"  ' Box, Cylinder, and some others.  Could be flagging sync parts.
        '                        ' These features do not have a FaceStyle override.

        '                    Case "1737031522"  'ConvertToSM
        '                        ' Doesn't work.
        '                        'Dim Feature_ = CType(Feature, SolidEdgePart.ConvToSM)
        '                        'FeatureFaceStyle = Feature_.GetStyle()
        '                        'If FeatureFaceStyle IsNot Nothing Then
        '                        '    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                        '   SolidEdgeGeometry.Faces)
        '                        '    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                        'End If

        '                    Case "1245666020"  'FaceSet
        '                        ' Doesn't work.
        '                        'Dim Feature_ = CType(Feature, SolidEdgePart.FaceSet)
        '                        'FeatureFaceStyle = Feature_.GetStyle()
        '                        'If FeatureFaceStyle IsNot Nothing Then
        '                        '    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                        '   SolidEdgeGeometry.Faces)
        '                        '    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                        'End If

        '                    Case "69347334"  'Hem
        '                        ' Doesn't work.
        '                        'Dim Feature_ = CType(Feature, SolidEdgePart.Hem)
        '                        'FeatureFaceStyle = Feature_.GetStyle()
        '                        'If FeatureFaceStyle IsNot Nothing Then
        '                        '    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                        '   SolidEdgeGeometry.Faces)
        '                        '    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                        'End If

        '                    Case "1477405962"  'Subtract
        '                        ' Doesn't work.
        '                        'Dim Feature_ = CType(Feature, SolidEdgePart.Subtract)
        '                        'FeatureFaceStyle = Feature_.GetStyle()
        '                        'If FeatureFaceStyle IsNot Nothing Then
        '                        '    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                        '   SolidEdgeGeometry.Faces)
        '                        '    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                        'End If

        '                    Case "-127107951"  'Thread
        '                        ' Doesn't work.
        '                        'Dim Feature_ = CType(Feature, SolidEdgePart.Thread)
        '                        'FeatureFaceStyle = Feature_.GetStyle()
        '                        'If FeatureFaceStyle IsNot Nothing Then
        '                        '    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                        '   SolidEdgeGeometry.Faces)
        '                        '    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                        'End If

        '                    Case "451898536"  'Transform.  Guessing it's ConvertPartToSM.
        '                        ' Doesn't work.
        '                        'Dim Feature_ = CType(Feature, SolidEdgePart.ConvertPartToSM)
        '                        'FeatureFaceStyle = Feature_.GetStyle()
        '                        'If FeatureFaceStyle IsNot Nothing Then
        '                        '    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                        '   SolidEdgeGeometry.Faces)
        '                        '    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                        'End If

        '                    Case "1385450842"  'Union
        '                        ' Doesn't work.
        '                        'Dim Feature_ = CType(Feature, SolidEdgePart.Union)
        '                        'FeatureFaceStyle = Feature_.GetStyle()
        '                        'If FeatureFaceStyle IsNot Nothing Then
        '                        '    FeatureFaces = CType(Feature_.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll),
        '                        '   SolidEdgeGeometry.Faces)
        '                        '    Return PopulateFeatureFaceOverrides(FeatureFaces, FeatureFaceStyle)
        '                        'End If

        '                    Case Else
        '                        Dim FeatureName = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetPropertyValue(Of String)(Feature, "Name")
        '                        ExitStatus = 1
        '                        ErrorMessageList.Add(String.Format("{0} (FeatureType={1}) not processed.  Please verify results.", FeatureName, FeatureType.ToString))
        '                End Select

        '        End Select

        '        Return FeatureFaceOverrides
        '    End Function

        'Dim UpdateFaces As Action(Of SolidEdgeFramework.FaceStyle) =
        '    Sub(_CurrentMaterialFaceStyle)
        '        Dim Model As SolidEdgePart.Model
        '        Dim Body As SolidEdgeGeometry.Body
        '        Dim Faces As SolidEdgeGeometry.Faces
        '        Dim Face As SolidEdgeGeometry.Face

        '        Dim Features As SolidEdgePart.Features

        '        Dim FaceOverrides As New Dictionary(Of Integer, SolidEdgeFramework.FaceStyle)
        '        Dim FeatureFaceOverrides As New Dictionary(Of Integer, SolidEdgeFramework.FaceStyle)
        '        Dim BodyOverride As SolidEdgeFramework.FaceStyle = Nothing

        '        Dim FeatureNames As New List(Of String)
        '        Dim FeatureName As String


        '        If (Models.Count > 0) And (Models.Count < 300) Then
        '            For Each Model In Models
        '                Features = Model.Features
        '                For Each Feature In Features
        '                    FeatureName = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetPropertyValue(Of String)(Feature, "Name")
        '                    If Not FeatureNames.Contains(FeatureName) Then
        '                        FeatureNames.Add(FeatureName)

        '                        FeatureFaceOverrides = GetFeatureFaceStyle(Feature)

        '                        If FeatureFaceOverrides.Count > 0 Then
        '                            For Each Key In FeatureFaceOverrides.Keys
        '                                FaceOverrides(Key) = FeatureFaceOverrides(Key)
        '                            Next
        '                        End If

        '                        FeatureFaceOverrides.Clear()
        '                    End If

        '                Next

        '                ' Some Models do not have a Body
        '                Try
        '                    Body = CType(Model.Body, SolidEdgeGeometry.Body)
        '                    If Body.Style IsNot Nothing Then
        '                        BodyOverride = Body.Style
        '                    End If

        '                    'Body.Faces
        '                    Faces = CType(Body.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll), SolidEdgeGeometry.Faces)

        '                    For Each Face In Faces
        '                        If Face.Style IsNot Nothing Then
        '                            FaceOverrides(Face.ID) = Face.Style
        '                        End If
        '                    Next

        '                    Dim MaxFacesToProcess As Integer = 500

        '                    If (FaceOverrides.Count > 0) And (FaceOverrides.Count <= MaxFacesToProcess) Then
        '                        ' Crashes on some imported files
        '                        Try
        '                            Body.ClearOverrides()
        '                            Body = CType(Model.Body, SolidEdgeGeometry.Body)
        '                            Faces = CType(Body.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll), SolidEdgeGeometry.Faces)
        '                        Catch ex As Exception
        '                            ExitStatus = 1
        '                            ErrorMessageList.Add("Body check interrupted.  Please verify results.")
        '                            Exit Sub
        '                        End Try

        '                        SEApp.DoIdle()

        '                        If BodyOverride IsNot Nothing Then
        '                            Body.Style = BodyOverride
        '                            BodyOverride = Nothing
        '                        End If

        '                        Dim Count As Integer = 0

        '                        For Each Face In Faces
        '                            If FaceOverrides.Keys.Contains(Face.ID) Then
        '                                Face.Style = FaceOverrides(Face.ID)
        '                                Count += 1
        '                                If Count Mod 100 = 0 Then
        '                                    SEApp.DoIdle()
        '                                End If
        '                            End If
        '                        Next
        '                        FaceOverrides.Clear()

        '                    ElseIf FaceOverrides.Count > MaxFacesToProcess Then
        '                        ExitStatus = 1
        '                        ErrorMessageList.Add(String.Format("{0} faces exceeds the maximum to process.  Please verify results.", FaceOverrides.Count.ToString))
        '                    End If

        '                Catch ex As Exception
        '                    ExitStatus = 1
        '                    ErrorMessageList.Add("Face check interrupted.  Please verify results.")
        '                    'Exit Sub
        '                End Try
        '            Next

        '        ElseIf Models.Count >= 300 Then
        '            ExitStatus = 1
        '            ErrorMessageList.Add(String.Format("{0} models exceeds maximum to process", Models.Count.ToString))
        '        End If

        '    End Sub

        'Dim MaxModelCount As Integer = 10

        'If (Models.Count > 0) And (Models.Count <= MaxModelCount) Then

        '    MatTable = SEApp.GetMaterialTable()

        '    ' This function populates 'CurrentMaterialName'
        '    MatTable.GetCurrentMaterialName(SEDoc, CurrentMaterialName)

        '    ' This function populates 'MaterialLibList' and 'NumMaterialLibraries'
        '    MatTable.GetMaterialLibraryList(MaterialLibList, NumMaterialLibraries)

        '    'Make sure the ActiveMaterialLibrary exists
        '    If Not IsActiveMaterialLibraryPresent(MaterialLibList, ActiveMaterialLibrary) Then
        '        Dim msg As String
        '        msg = "ActiveMaterialLibrary " + Configuration("TextBoxActiveMaterialLibrary") + " not found.  Exiting..." + Chr(13)
        '        msg += "Please update the Material Table on the Configuration tab." + Chr(13)
        '        MsgBox(msg)
        '        SEApp.Quit()
        '        End
        '    End If

        '    ' This function populates 'NumMaterials' and 'MaterialList'
        '    MatTable.GetMaterialListFromLibrary(ActiveMaterialLibrary, NumMaterials, MaterialList)

        '    If Not CurrentMaterialNameInLibrary(CurrentMaterialName, MaterialList) Then
        '        ExitStatus = 1
        '        If CurrentMaterialName = "" Then
        '            ErrorMessageList.Add(String.Format("Material 'None' not in {0}", ActiveMaterialLibrary))
        '        Else
        '            ErrorMessageList.Add(String.Format("Material '{0}' not in {1}", CurrentMaterialName, ActiveMaterialLibrary))
        '        End If
        '    Else
        '        For Each MatTableMaterial In CType(MaterialList, System.Array)
        '            If MatTableMaterial.ToString.ToLower.Trim = CurrentMaterialName.ToLower.Trim Then

        '                ' Names match, check if their properties do.
        '                If Not MaterialPropertiesMatch(MatTable, MatTableMaterial) Then

        '                    ' Properties do not match.  Update the document's material to match the library version.
        '                    MatTable.ApplyMaterialToDoc(SEDoc, MatTableMaterial.ToString, ActiveMaterialLibrary)
        '                    ExitStatus = 1
        '                    ErrorMessageList.Add(String.Format("'{0}' was updated", CurrentMaterialName))
        '                End If

        '                ' Face styles are not always updated, especially on imported files.
        '                ' Some imported files have trouble with face updates.
        '                Try
        '                    UpdateFaces(CurrentMaterialFaceStyle(MatTable, MatTableMaterial))
        '                    'MsgBox("UpdateFaces(CurrentMaterialFaceStyle(MatTable, MatTableMaterial))")
        '                Catch ex As Exception
        '                    ExitStatus = 1
        '                    ErrorMessageList.Add("Model check interrupted.  Please verify results.")
        '                End Try

        '                If SEDoc.ReadOnly Then
        '                    ExitStatus = 1
        '                    ErrorMessageList.Add("Cannot save document marked 'Read Only'")
        '                Else
        '                    SEDoc.Save()
        '                    SEApp.DoIdle()
        '                End If

        '                Exit For
        '            End If
        '        Next

        '    End If

        'ElseIf Models.Count >= MaxModelCount Then
        '    ExitStatus = 1
        '    ErrorMessageList.Add(String.Format("{0} models exceeds maximum to process", Models.Count.ToString))
        'End If


        'ErrorMessage(ExitStatus) = ErrorMessageList

        Dim ErrorMessageList As New List(Of String)
        Dim ExitStatus As Integer = 0
        Dim ErrorMessage As New Dictionary(Of Integer, List(Of String))

        Dim MaterialDoctorPart As New MaterialDoctorPart()

        ErrorMessage = MaterialDoctorPart.UpdateMaterialFromMaterialTable(SEDoc, Configuration, SEApp)

        Return ErrorMessage

    End Function


    Public Function UpdateFaceAndViewStylesFromTemplate(
        ByVal SEDoc As SolidEdgeFramework.SolidEdgeDocument,
        ByVal Configuration As Dictionary(Of String, String),
        ByVal SEApp As SolidEdgeFramework.Application
        ) As Dictionary(Of Integer, List(Of String))

        Dim ErrorMessage As New Dictionary(Of Integer, List(Of String))

        ErrorMessage = InvokeSTAThread(
                               Of SolidEdgePart.PartDocument,
                               Dictionary(Of String, String),
                               SolidEdgeFramework.Application,
                               Dictionary(Of Integer, List(Of String)))(
                                   AddressOf UpdateFaceAndViewStylesFromTemplateInternal,
                                   CType(SEDoc, SolidEdgePart.PartDocument),
                                   Configuration,
                                   SEApp)

        Return ErrorMessage

    End Function

    Private Function UpdateFaceAndViewStylesFromTemplateInternal(
        ByVal SEDoc As SolidEdgePart.PartDocument,
        ByVal Configuration As Dictionary(Of String, String),
        ByVal SEApp As SolidEdgeFramework.Application
        ) As Dictionary(Of Integer, List(Of String))

        Dim ErrorMessageList As New List(Of String)
        Dim ExitStatus As Integer = 0
        Dim ErrorMessage As New Dictionary(Of Integer, List(Of String))

        Dim SETemplateDoc As SolidEdgePart.PartDocument
        Dim Windows As SolidEdgeFramework.Windows
        Dim Window As SolidEdgeFramework.Window
        Dim View As SolidEdgeFramework.View
        Dim ViewStyles As SolidEdgeFramework.ViewStyles
        Dim ViewStyle As SolidEdgeFramework.ViewStyle

        Dim TemplateFilename As String = Configuration("TextBoxTemplatePart")
        Dim TemplateActiveStyleName As String = ""
        Dim TempViewStyleName As String = ""
        Dim ViewStyleAlreadyPresent As Boolean
        Dim TemplateSkyboxName(5) As String
        Dim msg As String = ""

        SEDoc.ImportStyles(TemplateFilename, True)

        ' Find the active ViewStyle in the template file.
        SETemplateDoc = CType(SEApp.Documents.Open(TemplateFilename), SolidEdgePart.PartDocument)
        SEApp.DoIdle()

        Windows = SETemplateDoc.Windows
        For Each Window In Windows
            View = Window.View
            TemplateActiveStyleName = View.Style.ToString
        Next

        ViewStyles = CType(SETemplateDoc.ViewStyles, SolidEdgeFramework.ViewStyles)

        For Each ViewStyle In ViewStyles
            If ViewStyle.StyleName = TemplateActiveStyleName Then
                For i As Integer = 0 To 5
                    TemplateSkyboxName(i) = ViewStyle.GetSkyboxSideFilename(i)
                Next
            End If
        Next

        SETemplateDoc.Close(False)
        SEApp.DoIdle()

        ' If a style by the same name exists in the target file, delete it.
        ViewStyleAlreadyPresent = False
        ViewStyles = CType(SEDoc.ViewStyles, SolidEdgeFramework.ViewStyles)
        For Each ViewStyle In ViewStyles
            If ViewStyle.StyleName = TemplateActiveStyleName Then
                ViewStyleAlreadyPresent = True
            Else
                TempViewStyleName = ViewStyle.StyleName
            End If
        Next

        SEApp.DoIdle()

        Windows = SEDoc.Windows

        If ViewStyleAlreadyPresent Then ' Hopefully deactivate the desired ViewStyle so it can be removed
            For Each Window In Windows
                View = Window.View
                View.Style = TempViewStyleName
            Next
            ViewStyles.Remove(TemplateActiveStyleName)
        End If

        ViewStyles.AddFromFile(TemplateFilename, TemplateActiveStyleName)

        For Each ViewStyle In ViewStyles
            If ViewStyle.StyleName = TemplateActiveStyleName Then
                ViewStyle.SkyboxType = SolidEdgeFramework.SeSkyboxType.seSkyboxTypeSkybox
                For i As Integer = 0 To 5
                    ViewStyle.SetSkyboxSideFilename(i, TemplateSkyboxName(i))
                Next
            End If
        Next

        For Each Window In Windows
            View = Window.View
            View.Style = TemplateActiveStyleName
        Next

        If SEDoc.ReadOnly Then
            ExitStatus = 1
            ErrorMessageList.Add("Cannot save document marked 'Read Only'")
        Else
            SEDoc.Save()
            SEApp.DoIdle()
        End If

        ErrorMessage(ExitStatus) = ErrorMessageList
        Return ErrorMessage
    End Function


    Public Function FitIsometricView(
        ByVal SEDoc As SolidEdgeFramework.SolidEdgeDocument,
        ByVal Configuration As Dictionary(Of String, String),
        ByVal SEApp As SolidEdgeFramework.Application
        ) As Dictionary(Of Integer, List(Of String))

        Dim ErrorMessage As New Dictionary(Of Integer, List(Of String))

        ErrorMessage = InvokeSTAThread(
                               Of SolidEdgePart.PartDocument,
                               Dictionary(Of String, String),
                               SolidEdgeFramework.Application,
                               Dictionary(Of Integer, List(Of String)))(
                                   AddressOf FitIsometricViewInternal,
                                   CType(SEDoc, SolidEdgePart.PartDocument),
                                   Configuration,
                                   SEApp)

        Return ErrorMessage

    End Function

    Private Function FitIsometricViewInternal(
        ByVal SEDoc As SolidEdgePart.PartDocument,
        ByVal Configuration As Dictionary(Of String, String),
        ByVal SEApp As SolidEdgeFramework.Application
        ) As Dictionary(Of Integer, List(Of String))

        Dim ErrorMessageList As New List(Of String)
        Dim ExitStatus As Integer = 0
        Dim ErrorMessage As New Dictionary(Of Integer, List(Of String))

        Dim RefPlanes As SolidEdgePart.RefPlanes
        Dim RefPlane As SolidEdgePart.RefPlane
        Dim Models As SolidEdgePart.Models

        Models = SEDoc.Models

        If Models.Count > 0 Then
            RefPlanes = SEDoc.RefPlanes
            For Each RefPlane In RefPlanes
                RefPlane.Visible = False
            Next
        Else
            RefPlanes = SEDoc.RefPlanes
            For Each RefPlane In RefPlanes
                RefPlane.Visible = True
            Next
        End If

        'Some imported files crash on this command
        Try
            SEDoc.Constructions.Visible = False
        Catch ex As Exception
        End Try

        SEDoc.CoordinateSystems.Visible = False

        SEApp.StartCommand(CType(SolidEdgeConstants.PartCommandConstants.PartViewISOView, SolidEdgeFramework.SolidEdgeCommandConstants))
        SEApp.StartCommand(CType(SolidEdgeConstants.PartCommandConstants.PartViewFit, SolidEdgeFramework.SolidEdgeCommandConstants))

        If SEDoc.ReadOnly Then
            ExitStatus = 1
            ErrorMessageList.Add("Cannot save document marked 'Read Only'")
        Else
            SEDoc.Save()
            SEApp.DoIdle()
        End If

        ErrorMessage(ExitStatus) = ErrorMessageList
        Return ErrorMessage
    End Function



End Class