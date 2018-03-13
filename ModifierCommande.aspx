<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ModifierCommande.aspx.cs" Inherits="ModifierCommande" %>
<%--JCOTE--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:AppConnectionString %>"></asp:SqlDataSource>
    <h1>Modification de commande</h1>
    <asp:Panel ID="PanelCommande" runat="server">
        <asp:Label ID="LabelInformationCommande" runat="server"></asp:Label>
        <br />
        <br />
        &nbsp;
        <asp:Label ID="LabelJeu" runat="server" Text="Jeu: "></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:DropDownList ID="DropDownListJeu" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListJeu_SelectedIndexChanged">
        </asp:DropDownList>
        <br />
        <br />
        &nbsp;
        <asp:Label ID="LabelEntrepot" runat="server" Text="Entrepot: "></asp:Label>
        <asp:DropDownList ID="DropDownListEntrepot" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListEntrepot_SelectedIndexChanged">
        </asp:DropDownList>
        <br />
        <br />
        &nbsp;
        <asp:Label ID="LabelQuantite" runat="server" Text="Quantité: "></asp:Label>
        <asp:DropDownList ID="DropDownListQuantite" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListQuantite_SelectedIndexChanged">
        </asp:DropDownList>
        <br />
        <br />
        &nbsp;
        <asp:Button ID="ButtonModifier" runat="server" Enabled="False" OnClick="ButtonModifier_Click" Text="Modifier" />
        <br />
        <br />
        &nbsp;
        <asp:Button ID="ButtonSupprimerCommande" runat="server" OnClick="ButtonSupprimerCommande_Click" Text="Supprimer" Width="114px" />
    </asp:Panel>
    <asp:Label ID="LabelConfirmation" runat="server"></asp:Label>
    <br />
    <br />
    <asp:Label ID="LabelRetour" runat="server" Text="Retour à la "></asp:Label>
    <a href="Administration.aspx">page d&#39;admninistration</a>
</asp:Content>
<%--JCOTE--%>

