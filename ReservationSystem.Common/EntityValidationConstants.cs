﻿namespace ReservationSystem.Common;

public static class EntityValidationConstants
{
    public static class Location
    {
        public const int LocationNameMinLength = 5;
        public const int LocationNameMaxLength = 30;

        public const int LocationAddressMinLength = 10;
        public const int LocationAddressMaxLength = 50;

        public const string LocationPricePerDayMinValue = "30";
        public const string LocationPricePerDayMaxValue = "90";

        public const int LocationDescriptionMinLength = 10;
        public const int LocationDescriptionMaxLength = 300;
    }

    public static class Reservation
    {
        public const int ReservationCustomersMinCount = 1;
        public const int ReservationCustomerMaxCount = 40;

        public const int ReservationAdditionalInfoMinLength = 5;
        public const int ReservationAdditionalInfoMaxLength = 150;
    }

    public static class PromoCode
    {
        public const int PromoCodeMinValue = 0;
        public const int PromoCodeMaxValue = 100;

        public const int PromoCodeMinLength = 4;
        public const int PromoCodeMaxLength = 10;
    }

    public static class Equipment
    {
        public const int EquipmentNameMinLength = 5;
        public const int EquipmentNameMaxLength = 30;
    }

    public static class Review
    {
        public const int ReviewCommentMinLength = 5;
        public const int ReviewCommentMaxLength = 500;
    }

    public static class User
    {
        public const int FirstNameMinLength = 1;
        public const int FirstNameMaxLength = 15;

        public const int LastNameMinLength = 1;
        public const int LastNameMaxLength = 15;
    }
}
