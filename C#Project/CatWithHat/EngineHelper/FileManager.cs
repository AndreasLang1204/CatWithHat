/****************************************************
 * Impressum                                        *
 * Studiengang: MultiMediaTechnology / FH-Salzburg  *
 * Zweck: MultiMediaProjekt 1 (MMP1)                *
 * Autor: Andreas Lang (FHS38594)                   *
 ****************************************************/
/**************************************************************************
* Implementierungsgrundlage für die Klasse wurde aus dem                  *
* “XNA Platformer” ­Tutorial von CodeingMadeEasy übernommen.               *     
* (https://www.youtube.com/watch?v=FR7crO2xq8A&list=PLE500D63CA505443B)   *
***************************************************************************/

#region Using Region
using System;
using System.Collections.Generic;
using System.IO;
#endregion

namespace MMP1
{
    public class FileManager
    {
        #region Enum Region
        enum LoatType { Attributes, Contents };
        #endregion

        #region Member Region
        LoatType type;
        List<List<string>> attributes;
        List<List<string>> contents;
        List<string> tempAttributes;
        List<string> tempContents;
        bool identifierFound = false;
        #endregion

        #region Property Region
        public List<List<string>> Attributes
        {
            get { return attributes; }
        }

        public List<List<string>> Contents
        {
            get { return contents; }
        }
        #endregion

        #region Constructor Region
        public FileManager()
        {
            attributes = new List<List<string>>();
            contents = new List<List<string>>();
        }
        #endregion

        #region Method Region
        /// <summary>
        /// Reads in a .cwh file and saves its content for further use
        /// </summary>
        /// <param name="filename">the file path</param>
        /// <param name="identifier"></param>
        public void LoadContent(string filename, string identifier)
        {
            using (StreamReader reader = new StreamReader(filename))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();

                    // check if the file contains an identifier
                    if (identifier == String.Empty)
                    {
                        identifierFound = true;
                    }
                    else if (line.Contains("EndLoad=") && line.Contains(identifier))
                    {
                        identifierFound = false;
                        break;
                    }
                    else if (line.Contains("Load=") && line.Contains(identifier))
                    {
                        identifierFound = true;
                        continue;
                    }
                  

                    if (identifierFound)
                    {
                        // read in attributes
                        if (line.Contains("Load="))
                        {
                            tempAttributes = new List<string>();
                            line = line.Remove(0, line.IndexOf("=") + 1);

                            type = LoatType.Attributes;
                        }
                        // read in the contents for the attributes
                        else
                        {
                            tempContents = new List<string>();

                            type = LoatType.Contents;
                        }

                        // split the line after each ']' to get the attribute/content values
                        string[] lineArray = line.Split(']');

                        foreach (string li in lineArray)
                        {
                            string newLine = li.Trim('[', ']');

                            if (newLine != String.Empty)
                            {
                                if (type == LoatType.Contents)
                                    tempContents.Add(newLine);
                                else
                                    tempAttributes.Add(newLine);
                            }
                        }

                        if (type == LoatType.Contents && tempContents.Count > 0)
                        {
                            contents.Add(tempContents);
                            attributes.Add(tempAttributes);
                        }
                    } 
                }
            }
        }
        #endregion
    }
}
