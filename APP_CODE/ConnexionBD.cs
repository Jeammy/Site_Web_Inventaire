using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OleDb;

/// <summary>
/// Description résumée de ConnexionBD
/// </summary>
public class ConnexionBD
{
    //Ce paramètre représente la connexion vers notre base de données Access
    OleDbConnection connection = null;
    
    /// <summary>
    /// Constructeur de la classe
    /// </summary>
	public ConnexionBD()
	{
        
	}

    /// <summary>
    /// Méthode qui établit la connexion vers la base de données Access avec le pilote installé
    /// </summary>
    /// <param name="connectionString">La chaîne de connexion qui contient le pilote, le nom de la base de données ainsi que les différents paramètres de connexion</param>
    /// <returns>Une OleDbConnection, représentant la connexion qui a été ouverte sur Access</returns>
    public OleDbConnection ConnectToDB(string connectionString)
    {
        //On initialise la connexion ici avec la chaîne de connexion qui provient du web.config, par l'entremise du formulaire qui nous appelle
        connection = new OleDbConnection(connectionString);
        //et on l'ouvre
        connection.Open();

        //On la retourne car il faut la passer au modèle pour continuer les opérations
        return connection;
    }

    /// <summary>
    /// Méthode qui permet de terminer la connexion
    /// </summary>
    public void CloseConnection()
    {
        //fermeture de la connexion, si existante !
        if (connection != null)
        {
            connection.Close();
        }
    }



}