using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OleDb;

/// <summary>
/// Description résumée de Modele     (ne pas se laissez fooled par le titre, le mot "client" n'a pas rapport)
/// </summary>
public class Modele
{
    //Ce paramètre représente la commande (l'enveloppe contenant la requête à effectuer auprès de la base de données
    OleDbCommand sql = null;
    //Ce paramètre représente le canal de communication pour faire passer l'enveloppe
    OleDbTransaction trans = null;

   
    /// <summary>
    /// Constructeur de la classe
    /// </summary>
    /// <param name="connection">C'est la connexion Access, on en a besoin dans la classe car on passera la transaction sur celle-ci</param>
	public Modele(OleDbConnection connection)
	{
        //On initialise la commande
        sql = new OleDbCommand();
        //et on lui indique sur quelle connexion elle sera faite
        sql.Connection = connection;
        //On ouvre le canal de communication
        trans = connection.BeginTransaction();
        //et on informe la commande qu'elle se fera sur celui-ci
        sql.Transaction = trans;
	}

    /// <summary>
    /// Creation d'un client dans la BD
    /// </summary>
    /// <param name="requete">La requête qui représente les champs et valeurs de création</param>
    /// <returns>un entier qui indique le nombre de rangées affectées par la requête</returns>
    public int CreateClient(string requete)
    {
        int numRows = 0;
        //On recoit la requete en paramètre, il faut la mettre dans l'enveloppe
        sql.CommandText = requete;
        //On exécute la requête auprès de la base de données et puisque cette méthode retourne le nombre de lignes impactés, nous allons le retourner à l'appelant
        numRows = sql.ExecuteNonQuery();
        
        return numRows;
    }

    /// <summary>
    /// Lecture d'un client à partir de la BD
    /// </summary>
    /// <param name="requete">La requête qui contient les critères de sélection</param>
    /// <returns>Un OleDbDataReader, l'ensemble de tous les enregistrements</returns>
    public OleDbDataReader ReadClient(string requete)
    {
        //On recoit la requete en paramètre, il faut la mettre dans l'enveloppe
        sql.CommandText = requete;
        //On exécute la requête auprès de la base de données
        sql.ExecuteNonQuery();
        //On retourne le reader, c'est à dire l'objet qui contient tous les enregistrements
        return sql.ExecuteReader();

        //pas nécessaire de faire un Commit car pas d'écriture à effectuer !
    }

    /// <summary>
    /// Méthode qui permet de faire marche arrière sur une transaction en cas de problème
    /// </summary>
    public void RollbackTransaction()
    {
        //Si le canal de communication existe, on le ferme
        if (trans != null)
        {
            trans.Rollback();
        }
    }

    /// <summary>
    /// Méthode qui permet d'écrire concrètement dans le fichier access toutes les manipulations en mémoire faites par les requêtes
    /// </summary>
    public void CommitChanges()
    {
        //Si le canal de communication existe, on écrit les changements dans la base de données
        if (trans != null)
        {
            trans.Commit();
        }
    }
}