using System;
using System.Collections.Generic;
using ShishaWeb.Models;
using ShishaWeb.MongoModels;
using System.Linq;

namespace ShishaWeb.Services
{
    public static class Mapper
    {
        public static ArticleDTO ToDto(this Article article)
        {
            return new ArticleDTO
            {
                Id = article.Id,
                Body = article.Content,
                CreatedAt = ((long)(article.CreatedAt - new DateTime(1970, 1, 1)).TotalMilliseconds).ToString(),
                CreatedBy = article.CreatedBy,
                LastModifiedAt = ((long)(article.CreatedAt - new DateTime(1970, 1, 1)).TotalMilliseconds).ToString(),
                LastModifiedBy = article.LastModifiedBy
            };
        }

        public static IEnumerable<ArticleDTO> ToDto(this IEnumerable<Article> articles)
        {
            var result = new List<ArticleDTO>();
            foreach (var article in articles)
            {
                result.Add(article.ToDto());
            }

            return result;
        }

        public static Article ToEntity(this ArticleDTO article)
        {
            return new Article
            {
                Id  = article.Id,
                Content = article.Body,
                //Picture = article.Picture,
                //Title = article.Title,
                CreatedAt = article.CreatedAt != null ? (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(article.CreatedAt)) : new DateTime(),
                CreatedBy = article.CreatedBy,
                LastModifiedAt = article.LastModifiedAt != null ? (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(article.LastModifiedAt)) : new DateTime(),
                LastModifiedBy = article.LastModifiedBy
            };
        }

        public static UserDTO ToDto(this User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Username = user.Username,
                PhotoUrl = user.PhotoUrl,
                Followers = user.Followers,
                Following = user.Following,
                FollowersCount = user.Followers.Length,
                FollowingCount = user.Following.Length,
                Badges = user.Badges.ToDto().ToArray(),
                UserGroup = user.UserGroup.ToString(),
                ShishasCreated = user.ShishasCreated,
                ShishasSmoked = user.ShishasSmoked,
                ShishasSmokedList = user.ShishasSmokedList,
                ShishasFavouriteList = user.ShishasFavouriteList,
                ShishasWishedList = user.ShishasWishedList,
                Settings = new UserSettingsDTO()
                {
                    IsActivityVisible = user.Settings.IsActivityVisible,
                    IsNargilesVisible = user.Settings.IsNargilesVisible,
                    IsProfileVisible = user.Settings.IsProfileVisible,
                    AllowNotifications = user.Settings.AllowNotifications,
                    Language = user.Settings.Language
                },
                CreatedAt = ((long)(user.CreatedAt - new DateTime(1970, 1, 1)).TotalMilliseconds).ToString(),
                CreatedBy = user.CreatedBy,
                LastModifiedAt = ((long)(user.CreatedAt - new DateTime(1970, 1, 1)).TotalMilliseconds).ToString(),
                LastModifiedBy = user.LastModifiedBy
            };
        }

        public static IEnumerable<UserDTO> ToDto(this IEnumerable<User> users)
        {
            var result = new List<UserDTO>();
            foreach (var user in users)
            {
                result.Add(user.ToDto());
            }

            return result;
        }

        public static User ToEntity(this UserDTO user)
        {
            return new User
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Username = user.Username,
                PhotoUrl = user.PhotoUrl,
                //Picture = user.Picture,
                Followers = user.Followers,
                Following = user.Following,
                Badges = user.Badges.ToEntity().ToArray(),
                UserGroup = (Config.Enumerations.UserGroupEnum)Enum.Parse(typeof(Config.Enumerations.UserGroupEnum), user.UserGroup),
                ShishasCreated = user.ShishasCreated,
                ShishasSmoked = user.ShishasSmoked,
                ShishasSmokedList = user.ShishasSmokedList,
                ShishasFavouriteList = user.ShishasFavouriteList,
                ShishasWishedList = user.ShishasWishedList,
                Settings = new UserSettings()
                {
                    IsActivityVisible = user.Settings.IsActivityVisible,
                    IsNargilesVisible = user.Settings.IsNargilesVisible,
                    IsProfileVisible = user.Settings.IsProfileVisible,
                    AllowNotifications = user.Settings.AllowNotifications,
                    Language = user.Settings.Language
                },
                CreatedAt = user.CreatedAt != null ? (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(user.CreatedAt)) : new DateTime(),
                CreatedBy = user.CreatedBy,
                LastModifiedAt = user.LastModifiedAt != null ? (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(user.LastModifiedAt)) : new DateTime(),
                LastModifiedBy = user.LastModifiedBy
            };
        }

        public static UserShortInfoDTO ToDto(this User user, bool shortInfo, IEnumerable<string> following)
        {
            return new UserShortInfoDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Username = user.Username,
                PhotoUrl = user.PhotoUrl,
                FollowersCount = user.Followers.Length,
                FollowingCount = user.Following.Length,
                IsFollowed = following.Contains(user.Id)
            };
        }

        public static IEnumerable<UserShortInfoDTO> ToDto(this IEnumerable<User> users, bool shortInfo, IEnumerable<string> following)
        {
            var result = new List<UserShortInfoDTO>();
            foreach (var user in users)
            {
                result.Add(user.ToDto(shortInfo, following));
            }

            return result;
        }

        public static UserBadgeDTO ToDto(this UserBadge badge)
        {
            return new UserBadgeDTO
            {
                Id = badge.Id,
                Name = badge.Name,
                Type = badge.Type
            };
        }

        public static IEnumerable<UserBadgeDTO> ToDto(this IEnumerable<UserBadge> badges)
        {
            var result = new List<UserBadgeDTO>();
            foreach (var badge in badges)
            {
                result.Add(badge.ToDto());
            }

            return result;
        }

        public static UserBadge ToEntity(this UserBadgeDTO badge)
        {
            return new UserBadge
            {
                Id = badge.Id,
                Name = badge.Name,
                Type = badge.Type
            };
        }

        public static IEnumerable<UserBadge> ToEntity(this IEnumerable<UserBadgeDTO> badges)
        {
            var result = new List<UserBadge>();
            foreach (var badge in badges)
            {
                result.Add(badge.ToEntity());
            }

            return result;
        }


        public static UserSettingsDTO ToDto(this UserSettings settings)
        {
            return new UserSettingsDTO
            {
                IsProfileVisible = settings.IsProfileVisible,
                IsActivityVisible = settings.IsActivityVisible,
                IsNargilesVisible = settings.IsNargilesVisible,
                AllowNotifications = settings.AllowNotifications,
                Language = settings.Language
            };
        }

        public static IEnumerable<UserSettingsDTO> ToDto(this IEnumerable<UserSettings> badges)
        {
            var result = new List<UserSettingsDTO>();
            foreach (var badge in badges)
            {
                result.Add(badge.ToDto());
            }

            return result;
        }

        public static UserSettings ToEntity(this UserSettingsDTO settings)
        {
            return new UserSettings
            {
                IsProfileVisible = settings.IsProfileVisible,
                IsActivityVisible = settings.IsActivityVisible,
                IsNargilesVisible = settings.IsNargilesVisible,
                AllowNotifications = settings.AllowNotifications,
                Language = settings.Language
            };
        }

        public static IEnumerable<UserSettings> ToEntity(this IEnumerable<UserSettingsDTO> badges)
        {
            var result = new List<UserSettings>();
            foreach (var badge in badges)
            {
                result.Add(badge.ToEntity());
            }

            return result;
        }

        public static TabaccoDTO ToDto(this Tabacco tabacco)
        {
            return new TabaccoDTO
            {
                Id = tabacco.Id,
                InternalId = tabacco.InternalId,
                Name = tabacco.Name,
                Type = tabacco.Type,
                Picture = tabacco.Picture,
                Power = tabacco.Power,
                CreatedAt = ((long)(tabacco.CreatedAt - new DateTime(1970, 1, 1)).TotalMilliseconds).ToString(),
                CreatedBy = tabacco.CreatedBy,
                LastModifiedAt = ((long)(tabacco.CreatedAt - new DateTime(1970, 1, 1)).TotalMilliseconds).ToString(),
                LastModifiedBy = tabacco.LastModifiedBy
            };
        }

        public static IEnumerable<TabaccoDTO> ToDto(this IEnumerable<Tabacco> tabaccos)
        {
            var result = new List<TabaccoDTO>();
            foreach (var tabacco in tabaccos)
            {
                result.Add(tabacco.ToDto());
            }

            return result;
        }

        public static Tabacco ToEntity(this TabaccoDTO tabacco)
        {
            return new Tabacco
            {
                Id = tabacco.Id,
                InternalId = tabacco.InternalId,
                Name = tabacco.Name,
                Type = tabacco.Type,
                Picture = tabacco.Picture,
                Power = tabacco.Power,
                CreatedAt = tabacco.CreatedAt != null ? (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(tabacco.CreatedAt)) : new DateTime(),
                CreatedBy = tabacco.CreatedBy,
                LastModifiedAt = tabacco.LastModifiedAt != null ? (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(tabacco.LastModifiedAt)) : new DateTime(),
                LastModifiedBy = tabacco.LastModifiedBy
            };
        }

        public static IEnumerable<Tabacco> ToEntity(this IEnumerable<TabaccoDTO> tabaccos)
        {
            var result = new List<Tabacco>();
            foreach (var tabacco in tabaccos)
            {
                result.Add(tabacco.ToEntity());
            }

            return result;
        }


        public static QrCodeDTO ToDto(this QrCode qrCode)
        {
            return new QrCodeDTO
            {
                Id = qrCode.Id,
                Value = qrCode.Value,
                Count = qrCode.Count,
                CreatedAt = ((long)(qrCode.CreatedAt - new DateTime(1970, 1, 1)).TotalMilliseconds).ToString(),
                CreatedBy = qrCode.CreatedBy,
                LastModifiedAt = ((long)(qrCode.CreatedAt - new DateTime(1970, 1, 1)).TotalMilliseconds).ToString(),
                LastModifiedBy = qrCode.LastModifiedBy
            };
        }

        public static IEnumerable<QrCodeDTO> ToDto(this IEnumerable<QrCode> qrCodes)
        {
            var result = new List<QrCodeDTO>();
            foreach (var qrCode in qrCodes)
            {
                result.Add(qrCode.ToDto());
            }

            return result;
        }

        public static QrCode ToEntity(this QrCodeDTO qrCode)
        {
            return new QrCode
            {
                Id = qrCode.Id,
                Value = qrCode.Value,
                Count = qrCode.Count,
                CreatedAt = qrCode.CreatedAt != null ? (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(qrCode.CreatedAt)) : new DateTime(),
                CreatedBy = qrCode.CreatedBy,
                LastModifiedAt = qrCode.LastModifiedAt != null ? (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(qrCode.LastModifiedAt)) : new DateTime(),
                LastModifiedBy = qrCode.LastModifiedBy
            };
        }

        public static ShishaDTO ToDto(this Shisha shisha) => new ShishaDTO
        {
            Id = shisha.Id,
            Identifier = shisha.Identifier,
            Name = shisha.Name,
            Description = shisha.Description,
            Type = shisha.Type,
            Tag = shisha.Tag,
            Picture = shisha.Picture,
            Power = shisha.Power,
            IsPublic = shisha.IsPublic,
            IsFavourite = shisha.IsFavourite,
            IsWish = shisha.IsWish,
            IsSeasonal = shisha.IsSeasonal,
            CreatedAt = ((long)(shisha.CreatedAt - new DateTime(1970, 1, 1)).TotalMilliseconds).ToString(),
            CreatedBy = shisha.CreatedBy,
            LastModifiedAt = ((long)(shisha.CreatedAt - new DateTime(1970, 1, 1)).TotalMilliseconds).ToString(),
            LastModifiedBy = shisha.LastModifiedBy
        };

        public static IEnumerable<ShishaDTO> ToDto(this IEnumerable<Shisha> shishas)
        {
            var result = new List<ShishaDTO>();
            foreach (var shisha in shishas)
            {
                result.Add(shisha.ToDto());
            }

            return result;
        }

        public static Shisha ToEntity(this ShishaDTO shisha) => new Shisha
        {
            Id = shisha.Id,
            Identifier = shisha.Identifier,
            Name = shisha.Name,
            Description = shisha.Description,
            Type = shisha.Type,
            Tag = shisha.Tag,
            Picture = shisha.Picture,
            Power = shisha.Power,
            IsPublic = shisha.IsPublic,
            IsFavourite = shisha.IsFavourite,
            IsWish = shisha.IsWish,
            IsSeasonal = shisha.IsSeasonal,
            Tabaccos = Mapper.ToEntity(shisha.Tabaccos).ToArray(),
            OwnRating = shisha.OwnRating,
            TotalRating = shisha.TotalRating,
            CreatedAt = shisha.CreatedAt != null ? (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(shisha.CreatedAt)) : new DateTime(),
            CreatedBy = shisha.CreatedBy,
            LastModifiedAt = shisha.LastModifiedAt != null ? (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(shisha.LastModifiedAt)) : new DateTime(),
            LastModifiedBy = shisha.LastModifiedBy
        };

    }
}
