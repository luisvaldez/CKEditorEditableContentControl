<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditableUserContent.ascx.cs" Inherits="luisvaldez.EditableContentControl.EditableUserContent" %>
<div class="ck-content" data-editable="true" data-editmode="<%=EditMode %>" data-maxlength="<%=MaxLength %>" data-sourceid="<%=ContentElement.SourceID%>"><%=ContentElement.HTML %></div>
<div runat="server" id="adminControls" style="display: flex; flex-flow: row nowrap; width: fit-content;">
    <div class="button contentElementSaveButton" runat="server" tabindex="0" id="SaveButton" data-role="Save" data-sourceid="<%=ContentElement.SourceID%>">{T:Save}</div>
    <div class="wordCount" data-sourceid="<%=ContentElement.SourceID%>"></div>
</div>
