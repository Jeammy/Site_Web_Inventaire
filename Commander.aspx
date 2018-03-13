<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Commander.aspx.cs" Inherits="Commander" %>
<%--JCOTE--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:AppConnectionString %>"></asp:SqlDataSource>
    <h1>Faire une commande</h1>
&nbsp;<asp:Panel ID="PanelCommande" runat="server">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="LabelTitre" runat="server" Text="Commande de Jeu"></asp:Label>
        <br />
        <br />
&nbsp;
        <asp:Label ID="LabelJeux" runat="server" Text="Jeu: "></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:DropDownList ID="DropDownListJeu" runat="server">
        </asp:DropDownList>
        <br />
        <br />
        &nbsp;
        <asp:Label ID="LabelEntrepot" runat="server" Text="Entrepôt: "></asp:Label>
        &nbsp;
        <asp:DropDownList ID="DropDownListEntrepot" runat="server">
        </asp:DropDownList>
        <br />
        <br />
        &nbsp;
        <asp:Label ID="LabelQuantite" runat="server" Text="Quantité: "></asp:Label>
        &nbsp;
        <asp:DropDownList ID="DropDownListQuantite" runat="server">
        </asp:DropDownList>
        <br />
        <br />
        &nbsp;
        <asp:Button ID="ButtonCommander" runat="server" Text="Commander" OnClick="ButtonCommander_Click" />
    </asp:Panel>
    <br />
    <asp:Label ID="LabelConfirmation" runat="server"></asp:Label>
    <br />
    <br />
    <asp:Label ID="LabelRetour" runat="server" Text="Retour à la "></asp:Label>
    <a href="Administration.aspx">page d&#39;admninistration</a>
    
</asp:Content>
<%--JCOTE--%>
