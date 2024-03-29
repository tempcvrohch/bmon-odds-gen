/*
 * BetMonitor for generating matches and Odds
 *
 * The server for simulating a betting portal.
 *
 * The version of the OpenAPI document: 1.0.0
 * Generated by: https://github.com/openapitools/openapi-generator.git
 */


using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using OpenAPIDateConverter = Org.OpenAPITools.Client.OpenAPIDateConverter;

namespace Org.OpenAPITools.Model
{
    /// <summary>
    /// A change during a match, most likely in points.
    /// </summary>
    [DataContract(Name = "match_upsert_dto_matchState")]
    public partial class MatchUpsertDtoMatchState : IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MatchUpsertDtoMatchState" /> class.
        /// </summary>
        /// <param name="pointScore">pointScore.</param>
        /// <param name="servingIndex">servingIndex.</param>
        /// <param name="setScore">setScore.</param>
        public MatchUpsertDtoMatchState(string pointScore = default(string), int servingIndex = default(int), string setScore = default(string))
        {
            this.PointScore = pointScore;
            this.ServingIndex = servingIndex;
            this.SetScore = setScore;
        }

        /// <summary>
        /// Gets or Sets PointScore
        /// </summary>
        [DataMember(Name = "pointScore", EmitDefaultValue = false)]
        public string PointScore { get; set; }

        /// <summary>
        /// Gets or Sets ServingIndex
        /// </summary>
        [DataMember(Name = "servingIndex", EmitDefaultValue = false)]
        public int ServingIndex { get; set; }

        /// <summary>
        /// Gets or Sets SetScore
        /// </summary>
        [DataMember(Name = "setScore", EmitDefaultValue = false)]
        public string SetScore { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class MatchUpsertDtoMatchState {\n");
            sb.Append("  PointScore: ").Append(PointScore).Append("\n");
            sb.Append("  ServingIndex: ").Append(ServingIndex).Append("\n");
            sb.Append("  SetScore: ").Append(SetScore).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            // ServingIndex (int) maximum
            if (this.ServingIndex > (int)1)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for ServingIndex, must be a value less than or equal to 1.", new [] { "ServingIndex" });
            }

            // ServingIndex (int) minimum
            if (this.ServingIndex < (int)0)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for ServingIndex, must be a value greater than or equal to 0.", new [] { "ServingIndex" });
            }

            yield break;
        }
    }

}
