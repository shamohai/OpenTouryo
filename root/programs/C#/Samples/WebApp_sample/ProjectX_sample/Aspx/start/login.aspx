﻿<%@ Page Language="C#" MasterPageFile="~/Aspx/Common/testBlankScreenNoJs.master" AutoEventWireup="true" Inherits="ProjectX_sample.Aspx.Start.login" Codebehind="login.aspx.cs" %>
<%@ Register Assembly="CustomControl" Namespace="Touryo.Infrastructure.CustomControl" TagPrefix="cc1" %>

<asp:Content ID="cphHeader" ContentPlaceHolderID="cphHeader" Runat="Server">
    <!-- Head 部の ContentPlaceHolder -->
</asp:Content>

<asp:Content ID="ContentPlaceHolder_A" ContentPlaceHolderID="ContentPlaceHolder_A" Runat="Server">    
    <div>
        <table id="Table1" border="1">
            <tr>
		        <td>ユーザID</td>
		        <td>
		            <cc1:WebCustomTextBox id="txtUserID" runat="server"></cc1:WebCustomTextBox>
                </td>
		    </tr>
		    <tr>
		        <td>パスワード</td>
		        <td>
		            <cc1:WebCustomTextBox id="txtPassword" runat="server" TextMode="Password"></cc1:WebCustomTextBox>
    		    </td>
		    </tr>
		    <tr>
	    	    <td colspan="2" align="right">
                    <cc1:WebCustomButton ID="btnButton1" runat="server" Text="ログイン" Width="150px" />
                    <!--<cc1:WebCustomButton ID="btnButton2" runat="server" Text="ログイン" Width="150px" />-->
                </td>
		    </tr>
	    </table>
	    <cc1:WebCustomLabel id="lblMessage" runat="server" Width="250px">Label</cc1:WebCustomLabel>    
    </div>
</asp:Content>

<asp:Content ID="cphFooter" ContentPlaceHolderID="cphFooter" Runat="Server">
    <!-- Footer 部の ContentPlaceHolder -->
</asp:Content>
