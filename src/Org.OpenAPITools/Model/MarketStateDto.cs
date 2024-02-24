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
    /// MarketStateDto
    /// </summary>
    [DataContract(Name = "market-state-dto")]
    public partial class MarketStateDto : IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MarketStateDto" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected MarketStateDto() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="MarketStateDto" /> class.
        /// </summary>
        /// <param name="id">id (required).</param>
        /// <param name="createdAt">createdAt.</param>
        /// <param name="updatedAt">updatedAt.</param>
        /// <param name="odd">odd (required).</param>
        /// <param name="suspended">suspended (required).</param>
        /// <param name="stakeLimit">stakeLimit.</param>
        /// <param name="market">market.</param>
        /// <param name="player">player.</param>
        public MarketStateDto(long id = default(long), DateTime createdAt = default(DateTime), DateTime updatedAt = default(DateTime), float odd = default(float), bool suspended = default(bool), int stakeLimit = default(int), MarketDto market = default(MarketDto), PlayerDto player = default(PlayerDto))
        {
            this.Id = id;
            this.Odd = odd;
            this.Suspended = suspended;
            this.CreatedAt = createdAt;
            this.UpdatedAt = updatedAt;
            this.StakeLimit = stakeLimit;
            this.Market = market;
            this.Player = player;
        }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name = "id", IsRequired = true, EmitDefaultValue = true)]
        public long Id { get; set; }

        /// <summary>
        /// Gets or Sets CreatedAt
        /// </summary>
        [DataMember(Name = "created_at", EmitDefaultValue = false)]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or Sets UpdatedAt
        /// </summary>
        [DataMember(Name = "updated_at", EmitDefaultValue = false)]
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Gets or Sets Odd
        /// </summary>
        [DataMember(Name = "odd", IsRequired = true, EmitDefaultValue = true)]
        public float Odd { get; set; }

        /// <summary>
        /// Gets or Sets Suspended
        /// </summary>
        [DataMember(Name = "suspended", IsRequired = true, EmitDefaultValue = true)]
        public bool Suspended { get; set; }

        /// <summary>
        /// Gets or Sets StakeLimit
        /// </summary>
        [DataMember(Name = "stakeLimit", EmitDefaultValue = false)]
        public int StakeLimit { get; set; }

        /// <summary>
        /// Gets or Sets Market
        /// </summary>
        [DataMember(Name = "market", EmitDefaultValue = false)]
        public MarketDto Market { get; set; }

        /// <summary>
        /// Gets or Sets Player
        /// </summary>
        [DataMember(Name = "player", EmitDefaultValue = false)]
        public PlayerDto Player { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class MarketStateDto {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  CreatedAt: ").Append(CreatedAt).Append("\n");
            sb.Append("  UpdatedAt: ").Append(UpdatedAt).Append("\n");
            sb.Append("  Odd: ").Append(Odd).Append("\n");
            sb.Append("  Suspended: ").Append(Suspended).Append("\n");
            sb.Append("  StakeLimit: ").Append(StakeLimit).Append("\n");
            sb.Append("  Market: ").Append(Market).Append("\n");
            sb.Append("  Player: ").Append(Player).Append("\n");
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
            // Odd (float) maximum
            if (this.Odd > (float)100)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for Odd, must be a value less than or equal to 100.", new [] { "Odd" });
            }

            // Odd (float) minimum
            if (this.Odd < (float)1)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for Odd, must be a value greater than or equal to 1.", new [] { "Odd" });
            }

            // StakeLimit (int) minimum
            if (this.StakeLimit < (int)0)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for StakeLimit, must be a value greater than or equal to 0.", new [] { "StakeLimit" });
            }

            yield break;
        }
    }

}
