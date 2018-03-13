<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
 
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:SqlDataSource ID="sqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:AppConnectionString %>"></asp:SqlDataSource>
    <!--<sspeichert>--> 
    <asp:Label ID="LabelBienvenue" runat="server" Text="Bonjour et bienvenue sur le site web de GameOn!" Font-Size="Larger"></asp:Label>
    <br />
    <br />
    <br />
    <asp:Label ID="LabelDescription" runat="server" Text="Ici, vous trouverez des jeux à vendre, dans un catalogue varié contenant autant du rétro que du nouveau,
        que ce soit des jeux d'actions, d'aventures, RPG, FPS et plus encore! Vous pourrez y découvrir les différents points de distribution, afin d'acquérir des 
        jeux de notre catalogue!"></asp:Label>
    <br />
    <br />
    <br />
    <br />
    Notre nouveauté vedette :<br />
    <video src="img/video/Far%20Cry%203%20-%20Stranded%20Trailer%20[UK].webm" controls="controls" />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
        <br />
    <br />
    <br />
    <br />
    <br />
        <br />
        <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />




    </video>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <asp:ListBox ID="ListBoxNouveautes" runat="server" Height="115px" Width="399px"></asp:ListBox>
    <br />
    <br />
    Filtrez la liste ci-dessous en sélectionnant un genre.<br />
    <asp:DropDownList ID="DropDownListGenre" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListGenre_SelectedIndexChanged">
        <asp:ListItem Value="0">Tous les genres</asp:ListItem>
    </asp:DropDownList>
    <br />
    <br />
     <asp:Table ID="TableClient" runat="server" BorderStyle="Solid" CellPadding="1" CellSpacing="1" GridLines="Both"></asp:Table>
    <br />
    
<!--</sspeichert>-->  


</asp:Content>

